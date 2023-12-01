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
    public class PostLikeService : IPostLikeService
    {
        private ForumContext context { get; set; }

        private ICacheProvider cacheProvider { get; set; }

        private ICommentService commentService { get; set; }


        public PostLikeService(ForumContext context, ICacheProvider cache, ICommentService commentService)
        {
            this.context = context;
            this.cacheProvider = cache;
            this.commentService = commentService;
        }

        #region LIKEPOST

        public async Task<bool> IsPostAlredyLikedAsync(int userId, int postId)
        {
            var postLike = await GetLikeAsync(userId, postId);

            if (postLike == null)
            {
                return false;
            }

            return true;
        }

        public async Task<int> RemoveLikeAsync(int userId, int postId)
        {
            var postLike = await GetLikeAsync(userId, postId);

            if (postLike == null)
            {
                throw new InvalidOperationException("Postlike doesnt exist for given userid and postid.");
            }

            context.LikePosts.Remove(postLike);
            await context.SaveChangesAsync();

            var allLikedPosts = context.LikePosts.Include(p => p.User).Where(p => p.Post.ID == postId).ToList();

            var likedPostCacheItem = new LikedDislikedPostCacheItem
            {
                Id = postId,
                UserIds = allLikedPosts.Select(p => p.User.ID).ToList()
            };

            cacheProvider.SetInHashSet("likedPosts", postId.ToString(), JsonSerializer.Serialize(likedPostCacheItem));

            return allLikedPosts.Count;

        }

        private async Task<LikePost> GetLikeAsync(int userId, int postId)
        {
            return await context.LikePosts.FirstOrDefaultAsync(p => p.User.ID == userId && p.Post.ID == postId);
        }

        public async Task<int> AddLikePostAsync(int userId, int postId)
        {
            if (context.LikePosts.FirstOrDefault(p => p.User.ID == userId && p.Post.ID == postId) != null)
            {
                throw new InvalidOperationException("Like for this post exist");
            }

            var user = await context.Users.FirstOrDefaultAsync(p => p.ID == userId);
            var post = await context.Posts.FirstOrDefaultAsync(p => p.ID == postId);

            if (user == null || post == null)
            {
                throw new InvalidOperationException("User or post doesnt exist.");
            }

            var likePost = new LikePost
            {
                User = user,
                Post = post
            };

            context.LikePosts.Add(likePost);
            await context.SaveChangesAsync();

            //Update in redis

            var likedPosts = await context.LikePosts.Include(p => p.User).Where(p => p.Post.ID == postId).ToListAsync();

            var likedDislikePostCacheItem = new LikedDislikedPostCacheItem
            {
                Id = postId,
                UserIds = likedPosts.Select(p => p.User.ID).ToList()
            };

            cacheProvider.SetInHashSet("likedPosts", postId.ToString(), JsonSerializer.Serialize(likedDislikePostCacheItem));


            return likedPosts.Count;
        }

        public async Task<object> LikePostAsync(int userId, int postId)
        {
            int? numberOfDislikes = null;

            if (await IsPostAlredyDisikedAsync(userId, postId))
            {
                numberOfDislikes = await RemoveDislikeAsync(userId, postId);

            }

            numberOfDislikes = (numberOfDislikes != null) ? numberOfDislikes : GetUsersWhoDislikedPost(postId).SelectMany(p => p.UserIds).Count();

            var numberOfLikes = await AddLikePostAsync(userId, postId);

            return new { likes = numberOfLikes , dislikes = numberOfDislikes };
        }

        public List<LikedDislikedPostModel> GetUsersWhoDislikedPost(int postId)
        {
            var cacheResult = cacheProvider.GetAllFromHashSet<LikedDislikedPostModel>("dislikedPosts");

            if (cacheResult.Count > 0)
            {
                var result = new List<LikedDislikedPostModel>();
                foreach (var likedPost in cacheResult)
                {
                    if (likedPost.Id == postId)
                    {
                        var resultItem = new LikedDislikedPostModel
                        {
                            Id = postId,
                            UserIds = likedPost.UserIds
                        };

                        result.Add(resultItem);
                    }
                }
                if (result.Any())
                {
                    return result;
                }
            }

            var allDislikedPosts = context.DislikePosts.Include(p => p.User).Where(p => p.Post.ID == postId).ToList();
            var dbResult = new List<LikedDislikedPostModel>();

            foreach (var likedPost in allDislikedPosts)
            {
                var dbResultItem = new LikedDislikedPostModel
                {
                    Id = postId,
                    UserIds = allDislikedPosts.Select(p => p.User.ID).ToList()
                };
                dbResult.Add(dbResultItem);

                var cacheModel = new LikedDislikedPostCacheItem
                {
                    Id = postId,
                    UserIds = allDislikedPosts.Select(p => p.User.ID).ToList()
                };
                cacheProvider.SetInHashSet("dislikedPosts", postId.ToString(), JsonSerializer.Serialize(cacheModel));
            }

            return dbResult;
        }

        public List<LikedDislikedPostModel> GetUsersWhoLikedPost(int postId)
        {
            var cacheResult = cacheProvider.GetAllFromHashSet<LikedDislikedPostModel>("likedPosts");

            if (cacheResult.Count > 0)
            {
                var result = new List<LikedDislikedPostModel>();
                foreach (var likedPost in cacheResult)
                {
                    if (likedPost.Id == postId)
                    {
                        var resultItem = new LikedDislikedPostModel
                        {
                            Id = postId,
                            UserIds = likedPost.UserIds
                        };

                        result.Add(resultItem);
                    }
                }
                if (result.Any())
                {
                    return result;
                }
            }

            var allLikedPosts = context.LikePosts.Include(p => p.User).Where(p => p.Post.ID == postId).ToList();
            var dbResult = new List<LikedDislikedPostModel>();

            foreach (var likedPost in allLikedPosts)
            {
                var dbResultItem = new LikedDislikedPostModel
                {
                    Id = postId,
                    UserIds = allLikedPosts.Select(p => p.User.ID).ToList()
                };
                dbResult.Add(dbResultItem);

                var cacheModel = new LikedDislikedPostCacheItem
                {
                    Id = postId,
                    UserIds = allLikedPosts.Select(p => p.User.ID).ToList()
                };
                cacheProvider.SetInHashSet("likedPosts", postId.ToString(), JsonSerializer.Serialize(cacheModel));
            }

            return dbResult;
        }


        #endregion

        #region DISLIKEPOST

        private async Task<DislikePost> GetDislikeAsync(int userId, int postId)
        {
            return await context.DislikePosts.FirstOrDefaultAsync(p => p.User.ID == userId && p.Post.ID == postId);
        }

        public async Task<int> AddDislikePostAsync(int userId, int postId)
        {
            if (context.DislikePosts.FirstOrDefault(p => p.User.ID == userId && p.Post.ID == postId) != null)
            {
                throw new InvalidOperationException("Dislike exist.");
            }

            var user = context.Users.FirstOrDefault(p => p.ID == userId);
            var post = context.Posts.FirstOrDefault(p => p.ID == postId);

            if (user == null || post == null)
            {
                throw new InvalidOperationException("USer or post doesnt exist.");
            }

            var dislike = new DislikePost
            {
                User = user,
                Post = post
            };

            context.DislikePosts.Add(dislike);
            await context.SaveChangesAsync();

            //Update in redis

            var dislikePost = await context.DislikePosts.Where(p => p.Post.ID == postId).ToListAsync();
            var likedDislikedPostCacheItem = new LikedDislikedPostCacheItem
            {
                Id = postId,
                UserIds = dislikePost.Select(p => p.UserID).ToList()
            };

            cacheProvider.SetInHashSet("dislikedPosts", postId.ToString(), JsonSerializer.Serialize(likedDislikedPostCacheItem));


            return dislikePost.Count;
        }

        public async Task<object> DislikePostAsync(int userId, int postId)
        {
            int? numberOfLikes = null;

            if (await IsPostAlredyLikedAsync(userId, postId))
            {
                numberOfLikes = await RemoveLikeAsync(userId, postId);
            }

            var numberOfDislikes = await AddDislikePostAsync(userId, postId);

            numberOfLikes = numberOfLikes != null ? numberOfLikes : GetUsersWhoLikedPost(postId).SelectMany(p => p.UserIds).Count();

            return new { likes = numberOfLikes, dislikes = numberOfDislikes };
        }

        public async Task<int> RemoveDislikeAsync(int userId, int postId)
        {
            var postDislike = await GetDislikeAsync(userId, postId);

            if (postDislike == null)
            {
                throw new InvalidOperationException("Postlike doesnt exist for given userid and postid.");
            }

            context.DislikePosts.Remove(postDislike);
            await context.SaveChangesAsync();

            var allDislikePosts = context.DislikePosts.Include(p => p.User).Where(p => p.Post.ID == postId).ToList();

            var dislikePostCacheItem = new LikedDislikedPostCacheItem
            {
                Id = postId,
                UserIds = allDislikePosts.Select(p => p.User.ID).ToList()
            };

            cacheProvider.SetInHashSet("dislikedPosts", postId.ToString(), JsonSerializer.Serialize(dislikePostCacheItem));

            return allDislikePosts.Count;
        }

        public async Task<bool> IsPostAlredyDisikedAsync(int userId, int postId)
        {
            var postDislike = await GetDislikeAsync(userId, postId);

            if (postDislike == null)
            {
                return false;
            }

            return true;
        }


        #endregion

        public async Task<PostReactionsModel> GetPostReactionsAsync(int postId)
        {
            return await context.Posts
                               .Include(p => p.Likes)
                               .Include(p => p.Dislikes)
                               .Where(p => p.ID == postId)
                               .Select(p => new PostReactionsModel
                               {
                                   Id = p.ID,
                                   LikesCount = p.Likes.Count,
                                   DislikesCount= p.Dislikes.Count
                               })
                               .FirstOrDefaultAsync();
        }

        private async Task<object> GetPost(int postId)
        {
            return await context.Posts
                .Include(p => p.Likes)
                .Include(p => p.Dislikes)
                .Where(p => p.ID == postId)
                .Select(p => new
                {
                    likes = p.Likes.Count,
                    dislikes = p.Dislikes.Count
                })
                .FirstOrDefaultAsync();
        }

        private static string TimeAgo(DateTime dateTime)
        {
            string result = string.Empty;
            var timeSpan = DateTime.Now.Subtract(dateTime);

            if (timeSpan <= TimeSpan.FromSeconds(60))
            {
                result = string.Format("{0} seconds ago", timeSpan.Seconds);
            }
            else if (timeSpan <= TimeSpan.FromMinutes(60))
            {
                result = timeSpan.Minutes > 1 ?
                    String.Format("about {0} minutes ago", timeSpan.Minutes) :
                    "about a minute ago";
            }
            else if (timeSpan <= TimeSpan.FromHours(24))
            {
                result = timeSpan.Hours > 1 ?
                    String.Format("about {0} hours ago", timeSpan.Hours) :
                    "about an hour ago";
            }
            else if (timeSpan <= TimeSpan.FromDays(30))
            {
                result = timeSpan.Days > 1 ?
                    String.Format("about {0} days ago", timeSpan.Days) :
                    "yesterday";
            }
            else if (timeSpan <= TimeSpan.FromDays(365))
            {
                result = timeSpan.Days > 30 ?
                    String.Format("about {0} months ago", timeSpan.Days / 30) :
                    "about a month ago";
            }
            else
            {
                result = timeSpan.Days > 365 ?
                    String.Format("about {0} years ago", timeSpan.Days / 365) :
                    "about a year ago";
            }

            return result;
        }
    }
}
