using ForumApp.Cache;
using ForumApp.Cache.Classes;
using ForumApp.Core.Context;
using ForumApp.Core.Dtos;
using ForumApp.Core.Interfaces;
using ForumApp.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ForumApp.Services.Implementations
{
    public class CommentService : ICommentService
    {
        private ForumContext context { get; set; }

        private ICacheProvider cacheProvider { get; set; }

        public CommentService(ForumContext context, ICacheProvider cacheProvider)
        {
            this.context = context;
            this.cacheProvider = cacheProvider;
        }

        public List<CommentModel> GetCommentsForPost(int postId)
        {
            var redisResult = cacheProvider.GetAllFromHashSet<CommentCacheItem>("comments")
                                           .Where(p => p.PostId == postId)
                                           .ToList();

            if (redisResult.Count > 0)
            {
                var result = new List<CommentModel>();

                foreach (var commnetItem in redisResult)
                {
                    var commentModel = new CommentModel
                    {
                        Id = commnetItem.Id,
                        Text = commnetItem.Text,
                        PostId = commnetItem.PostId,
                        UserId = commnetItem.UserId,
                        UserWhoLikedComment = GetUsersWhoLikedComment(commnetItem.Id).SelectMany(p => p.UserIds).ToList(),
                        UserWhoDislikedComment = GetUsersWhoDislikeComment(commnetItem.Id).SelectMany(p => p.UserIds).ToList()
                    };

                    result.Add(commentModel);
                }

                return result;
            }

            var commnetsForPost = context.Comments
                                                  .Where(p => p.Post.ID == postId)
                                                  .Select(p => new CommentModel
                                                  {
                                                      Id = p.ID,
                                                      Text = p.Text,
                                                      PostId = p.Post.ID,
                                                      UserId = p.User.ID
                                                  })
                                                  .ToList();

            foreach (var item in commnetsForPost)
            {
                item.UserWhoLikedComment = GetUsersWhoLikedComment(item.Id).SelectMany(p => p.UserIds).ToList();
                item.UserWhoDislikedComment = GetUsersWhoDislikeComment(item.Id).SelectMany(p => p.UserIds).ToList();

                var itemInCache = new CommentCacheItem
                {
                    Id = item.Id,
                    Text = item.Text,
                    PostId = item.PostId,
                    UserId = item.UserId
                };

                cacheProvider.SetInHashSet("comments", item.Id.ToString(), JsonSerializer.Serialize(itemInCache));
            }

            return commnetsForPost;
        }
        
        public async Task AddCommentAsync(int userId, int postId, string textComment)
        {
            var user = await GetUserAsync(userId);
            var post = await GetPostAsync(postId);

            if (user == null)
            {
                throw new InvalidOperationException("User don't exist");
            }

            if (post == null)
            {
                throw new InvalidOperationException("Post don't exist");
            }

            var comment = new Comment
            {
                User = user,
                Post = post,
                Text = textComment
            };

            context.Comments.Add(comment);
            await context.SaveChangesAsync();

            
            var commentCacheItem = new CommentCacheItem
            {
                Id = comment.ID,
                PostId = postId,
                Text = textComment,
                UserId = userId
            };

            cacheProvider.SetInHashSet("comments", comment.ID.ToString(), JsonSerializer.Serialize(commentCacheItem));
        }

        public async Task RemoveCommentAsync(int userId, int commentId)
        {
            var user = await GetUserAsync(userId);

            if (user == null)
            {
                throw new InvalidOperationException("User don't exist");
            }

            var comment = await context.Comments
                .Include(p => p.DislikeComments)
                .Include(p => p.LikeComments)
                .FirstOrDefaultAsync(p => p.ID == commentId && p.User.ID == userId);

            if (comment == null)
            {
                throw new InvalidOperationException("You don't have permition to delete that comment");
            }

            context.Comments.Remove(comment);
            await context.SaveChangesAsync();

            cacheProvider.DeleteFromHashSet("comments", commentId.ToString());

        }

        public List<LikedDislikedCommentModel> GetUsersWhoLikedComment(int commentId)
        {
            var cacheResult = cacheProvider.GetAllFromHashSet<LikedDislikedCommentCacheItem>("likedComments");

            if (cacheResult.Count > 0)
            {
                var result = new List<LikedDislikedCommentModel>();
                foreach (var likedComment in cacheResult)
                {
                    if(likedComment.Id == commentId)
                    {
                        var resultItem = new LikedDislikedCommentModel
                        {
                            Id = commentId,
                            UserIds = likedComment.UserIds
                        };

                        result.Add(resultItem);
                    }
                }
                if(result.Any())
                {
                    return result;
                }    
            }

            var allLikedComments = context.LikeComments.Where(p => p.CommnetID == commentId).ToList();
            var dbResult = new List<LikedDislikedCommentModel>();
            
            foreach (var likedComment in allLikedComments)
            {
                var dbResultItem = new LikedDislikedCommentModel
                {
                    Id = commentId,
                    UserIds = allLikedComments.Select(p => p.UserID).ToList()
                };
                dbResult.Add(dbResultItem);

                var cacheModel = new LikedDislikedCommentCacheItem
                {
                    Id = commentId,
                    UserIds = allLikedComments.Select(p => p.UserID).ToList()
                };
                cacheProvider.SetInHashSet("likedComments", commentId.ToString(), JsonSerializer.Serialize(cacheModel));
            }

            return dbResult;
        }

        public List<LikedDislikedCommentModel> GetUsersWhoDislikeComment(int commentId)
        {
            var dislikeCommentCache = cacheProvider.GetAllFromHashSet<LikedDislikedCommentCacheItem>("dislikedComments");

            if(dislikeCommentCache.Any())
            {
                var result = new List<LikedDislikedCommentModel>();
                
                foreach(var comm in dislikeCommentCache)
                {
                    if (comm.Id == commentId)
                    {
                        var resultItem = new LikedDislikedCommentModel
                        {
                            Id = commentId,
                            UserIds = comm.UserIds
                        };

                        result.Add(resultItem);
                    }
                }
                if (result.Any())
                {
                    return result;
                }
            }

            var allDislikeComment = context.DislikeComments.Include(p => p.User).Where(p => p.Commnet.ID == commentId).ToList();
            var dbResult = new List<LikedDislikedCommentModel>();

            foreach (var comm in allDislikeComment)
            {
                var result = new LikedDislikedCommentModel
                {
                    Id = comm.ID,
                    UserIds = allDislikeComment.Select(p => p.User.ID).ToList()
                };
                dbResult.Add(result);

                var cacheModel = new LikedDislikedCommentCacheItem
                {
                    Id = comm.ID,
                    UserIds = allDislikeComment.Select(p => p.User.ID).ToList()
                };
                cacheProvider.SetInHashSet("dislikedComments", commentId.ToString(), JsonSerializer.Serialize(cacheModel));
            }
            return dbResult;
        }            

        private async Task<User> GetUserAsync(int userId)
        {
            return await context.Users.FirstOrDefaultAsync(p => p.ID == userId);
        }

        private async Task<Post> GetPostAsync(int postID)
        {
            return await context.Posts.FirstOrDefaultAsync(p => p.ID == postID);
        }


    }
}
