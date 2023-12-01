using ForumApp.Cache;
using ForumApp.Cache.Classes;
using ForumApp.Core.Context;
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
    public class CommentLikeOrDislikeService : ICommentLikeOrDislikeService
    {
        private ForumContext context;

        private ICacheProvider cacheProvider; 

        public CommentLikeOrDislikeService(ForumContext context, ICacheProvider cache)
        {
            this.context = context;
            cacheProvider = cache;
        }

      
        #region DISLIKECOMMENT

        private async Task<DislikeComment> GetDislikeCommentAsync(int userId, int commentId)
        {
            return await context.DislikeComments.FirstOrDefaultAsync(p => p.User.ID == userId && p.Commnet.ID == commentId);
        }

        public async Task AddDislikeCommentAsync(int userId, int commentId)
        {
            var dislikeComment = await GetDislikeCommentAsync(userId, commentId);
            if (dislikeComment != null)
            {
                throw new InvalidOperationException("Dislike for this comment already exist!");
            }

            var user = await context.Users.FindAsync(userId);
            var comment = await context.Comments.FindAsync(commentId);

            if (user == null || comment == null)
            {
                throw new InvalidOperationException("User or comment not exist!");
            }

            var dislikeComm = new DislikeComment
            {
                User = user,
                Commnet = comment
            };


            context.DislikeComments.Add(dislikeComm);
            await context.SaveChangesAsync();

            //Update in redis

            var userWhoDislikeComment = await context.DislikeComments.Where(p => p.Commnet.ID == commentId).ToListAsync();
            var likedDislikeCommentCacheItem = new LikedDislikedCommentCacheItem
            {
                Id = commentId,
                UserIds = userWhoDislikeComment.Select(p => p.UserID).ToList()
            };

            cacheProvider.SetInHashSet("dislikedComments", commentId.ToString(), JsonSerializer.Serialize(likedDislikeCommentCacheItem));

        }

        public async Task RemoveDislikeCommentAsync(int userId, int commentId)
        {
            var dislikeComment = await GetDislikeCommentAsync(userId, commentId);
            if (dislikeComment == null)
            {
                throw new InvalidOperationException("Dislike for this comment does not exist!");
            }

            context.DislikeComments.Remove(dislikeComment);
            await context.SaveChangesAsync();
        }

        public async Task<bool> IsCommentAlreadyDislikeAsync(int userId, int commentId)
        {
            var dislikeComm = await GetDislikeCommentAsync(userId, commentId);
            if (dislikeComm == null)
            {
                return false;
            }
            return true;
        }

        public async Task DislikeCommentAsync(int userId, int commentId)
        {
            if (await IsCommentAlreadyLikeAsync(userId, commentId))
            {
                await RemoveLikeCommentAsync(userId, commentId);

                var allLikedComment = await context.LikeComments.Include(p => p.User).Where(p => p.Commnet.ID == commentId).ToListAsync();
                var likedDislikeCommentCacheItem = new LikedDislikedCommentCacheItem
                {
                    Id = commentId,
                    UserIds = allLikedComment.Select(p => p.UserID).ToList()
                };

                cacheProvider.SetInHashSet("likedComments", commentId.ToString(), JsonSerializer.Serialize(likedDislikeCommentCacheItem));
            }

            await AddDislikeCommentAsync(userId, commentId);
        }

        #endregion


        #region LIKECOMMENT

        public async Task AddLikeCommentAsync(int userId, int commentId)
        {
            var user = await GetUserAsync(userId);

            var comment = await GetCommentAsync(commentId);

            if (user == null || comment == null)
            {
                throw new InvalidOperationException("Doesn't exist comment or user");
            }

            if (await context.LikeComments.FirstOrDefaultAsync(p => p.User.ID == userId && p.Commnet.ID == commentId) != null)
            {
                throw new InvalidOperationException("Comments is already like");
            }

            var like = new LikeComment
            {
                User = user,
                Commnet = comment
            };

            context.LikeComments.Add(like);
            await context.SaveChangesAsync();
            
            //Update in redis

            var userWhoLikedComment = await context.LikeComments.Where(p => p.Commnet.ID == commentId).ToListAsync();
            var likedDislikeCommentCacheItem = new LikedDislikedCommentCacheItem
            {
                Id = commentId,
                UserIds = userWhoLikedComment.Select(p => p.UserID).ToList()
            };

            cacheProvider.SetInHashSet("likedComments", commentId.ToString(), JsonSerializer.Serialize(likedDislikeCommentCacheItem));
        }

        public async Task RemoveLikeCommentAsync(int userId, int commentId)
        {
            var like = await LikeIsAlreadyExistAsync(userId, commentId);
            if (like != null)
            {
                context.LikeComments.Remove(like);
                await context.SaveChangesAsync();
            }

            var userWhoLikedComment = await context.LikeComments.Include(p => p.User).Where(p => p.Commnet.ID == commentId).ToListAsync();
            var likedDislikeCommentCacheItem = new LikedDislikedCommentCacheItem
            {
                Id = commentId,
                UserIds = userWhoLikedComment.Select(p => p.UserID).ToList()
            };

            cacheProvider.SetInHashSet("likedComments", commentId.ToString(), JsonSerializer.Serialize(likedDislikeCommentCacheItem));

        }

        private async Task<LikeComment> LikeIsAlreadyExistAsync(int userId, int commentId)
        {
            var like = await context.LikeComments
                    .Where(p => p.User.ID == userId && p.Commnet.ID == commentId)
                    .FirstOrDefaultAsync();

            return like;
        }

        private async Task<User> GetUserAsync(int userId)
        {
            return await context.Users.FirstOrDefaultAsync(p => p.ID == userId);
        }

        private async Task<Comment> GetCommentAsync(int commentId)
        {
            return await context.Comments.FirstOrDefaultAsync(p => p.ID == commentId);
        }

        public async Task<bool> IsCommentAlreadyLikeAsync(int userId, int commentId)
        {
            var likeComm = await LikeIsAlreadyExistAsync(userId, commentId);
            if (likeComm == null)
            {
                return false;
            }
            return true;
        }

        public async Task LikeCommentAsync(int userId, int commentId)
        {
            if (await IsCommentAlreadyDislikeAsync(userId, commentId))
            {
                await RemoveDislikeCommentAsync(userId, commentId);

                //Update in REDIS

                var allDislikedComment = await context.DislikeComments.Include(p => p.User).Where(p => p.Commnet.ID == commentId).ToListAsync();
                var likedDislikeCommentCacheItem = new LikedDislikedCommentCacheItem
                {
                    Id = commentId,
                    UserIds = allDislikedComment.Select(p => p.UserID).ToList()
                };

                cacheProvider.SetInHashSet("dislikedComments", commentId.ToString(), JsonSerializer.Serialize(likedDislikeCommentCacheItem));

            }

            await AddLikeCommentAsync(userId, commentId);
        }

        #endregion
    }
}

