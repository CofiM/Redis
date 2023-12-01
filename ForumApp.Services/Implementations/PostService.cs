using ForumApp.Cache;
using ForumApp.Cache.Classes;
using ForumApp.Core.Context;
using ForumApp.Core.Dtos;
using ForumApp.Core.Interfaces;
using ForumApp.Core.Interfaces.CacheServices;
using ForumApp.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ForumApp.Services.Implementations
{
    public class PostService : IPostService
    {
        private ForumContext context { get; set; }

        private ICacheProvider cacheProvider { get; set; } 

        private ICommentService commentService { get; set; }

        private IPostLikeService postLikeService { get; set; }

        private ICommunityService communityService { get; set; }

        private IFollowedCommunityServices followedCommunityServices { get; set; }

        private ICommentPostCacheService commentPostCacheService { get; set; }


        public PostService(ForumContext context, ICacheProvider cacheProvider, ICommentService commentService, IPostLikeService postLikeService, ICommunityService communityService, IFollowedCommunityServices followedCommunityServices, ICommentPostCacheService commentPostCacheService)
        {
            this.context = context;
            this.cacheProvider = cacheProvider;
            this.commentService = commentService;
            this.postLikeService = postLikeService;
            this.communityService = communityService;
            this.followedCommunityServices = followedCommunityServices;
            this.commentPostCacheService = commentPostCacheService;
        }

        private static string TimeAgo (DateTime dateTime)
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

        public async Task AddPost(string title, string text, string topic, int userId, int communityId)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new Exception("Title is invalid.");
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new Exception("Text is invalid.");
            }

            if (string.IsNullOrWhiteSpace(topic))
            {
                throw new Exception("Topic is invalid.");
            }

            if (userId < 0)
            {
                throw new Exception("Id of user is invalid.");
            }

            if (communityId < 0)
            {
                throw new Exception("Id of community is invalid.");
            }

            var post = new Post();
            post.Text = text;
            post.Title = title;

            post.Date = DateTime.Now;

            var user = await context.Users.FindAsync(userId);
            post.User = user;

            var community = await context.Communities.FindAsync(communityId);
            post.Community = community;

            if (topic != "---")
            {
                var wantedTopic = await context.Topics.Where(p => p.Name == topic).FirstOrDefaultAsync();
                if (wantedTopic == null)
                {
                    var newTopic = new Topic();
                    newTopic.Name = topic;
                    context.Topics.Add(newTopic);
                    await context.SaveChangesAsync();
                    post.Topic = newTopic;
                }
                else
                {
                    post.Topic = wantedTopic;
                }
            }
            context.Posts.Add(post);
            await context.SaveChangesAsync();

            //Update in redis

            var allPosts = await context.Posts
                .Include(p => p.Likes)
                .Include(p => p.Dislikes)
                .Where(p => p.Community.ID == communityId).ToListAsync();

            var result = new List<PostCardModel>();

            foreach(var item in allPosts)
            {
                var postCardModel = new PostCardModel
                {
                    Id = item.ID,
                    Title = item.Title,
                    Text = item.Text,
                    Likes = item.Likes.Count,
                    Dislikes = item.Dislikes.Count,
                    CommunityName = item.Community.Name,
                    CommunityId = item.Community.ID,
                    Date = TimeAgo(item.Date),
                    CommentsCount = commentPostCacheService.GetCommentsCount(item.ID),
                    //UsersWhoLikedThisPost = GetUsersWhoLikedPost(item.ID).SelectMany(p => p.UserIds).ToList().Any() ? GetUsersWhoLikedPost(item.ID).SelectMany(p => p.UserIds).ToList() : new List<int>(),
                    //UsersWhoDislikedThisPost = GetUsersWhoDislikedPost(item.ID).SelectMany(p => p.UserIds).ToList().Any() ? GetUsersWhoDislikedPost(item.ID).SelectMany(p => p.UserIds).ToList() : new List<int>()
                    UsersWhoLikedThisPost = GetUsersWhoLikedPost(item.ID)?.UserIds,
                    UsersWhoDislikedThisPost = GetUsersWhoDislikedPost(item.ID)?.UserIds

                };

                result.Add(postCardModel);
            }

            foreach(var res in result)
            {
                cacheProvider.SetInHashSet("posts", res.Id.ToString(), JsonSerializer.Serialize(res));
            }
        }

        public async Task<List<object>> GetPostsForCommunity(int communityId, int userId)
        {

            var container = new List<object>();

            var posts = await context.Posts
                                .Include(p => p.Likes)
                                .Include(p => p.Dislikes)
                                .Include(p => p.Comments)
                                .ThenInclude(p => p.LikeComments)
                                .Include(p => p.Comments)
                                .ThenInclude(p => p.DislikeComments)
                                .Where(p => p.Community.ID == communityId)
                                .Select(p => new
                                {
                                    p.ID,
                                    p.Title,
                                    p.Text,
                                    Likes = p.Likes.Count,
                                    Dislikes = p.Dislikes.Count,
                                    Comments = p.Comments.Select(q => new
                                    {
                                        q.ID,
                                        q.Text,
                                        LikesComment = q.LikeComments.Count,
                                        DislikeComment = q.DislikeComments.Count
                                    }),
                                    CommentsCount = p.Comments.Count,
                                    Date = TimeAgo(p.Date)
                                })
                                .ToListAsync();

            foreach (var post in posts)
            {
                var isPostLiked = false;
                var isPostDisliked = false;

                if (await context.LikePosts.Where(p => p.User.ID == userId && p.Post.ID == post.ID).FirstOrDefaultAsync() != null)
                {
                    isPostLiked = true;
                }

                if (await context.DislikePosts.Where(p => p.User.ID == userId && p.Post.ID == post.ID).FirstOrDefaultAsync() != null)
                {
                    isPostDisliked = true;
                }

                object term = new
                {
                    post,
                    isPostLiked,
                    isPostDisliked
                };

                container.Add(term);
            }
            return container;
        }

        public async Task<List<Post>> GetPostsForTopic(int topicId)
        {
            var posts = await context.Posts
                   .Include(p => p.Likes)
                   .Include(p => p.Dislikes)
                   .Include(p => p.Comments)
                   .ThenInclude(p => p.LikeComments)
                   .Include(p => p.Comments)
                   .ThenInclude(p => p.DislikeComments)
                   .Where(p => p.Topic.ID == topicId)
                   .ToListAsync();

            if (posts == null)
                throw new Exception("Posts not found for topic");

            return posts;
        }

        private async Task<List<PostCardModel>> GetPostsByCommunityAsync(int communityId)
        {
            var postsCache = cacheProvider.GetAllFromHashSet<PostCacheItem>("posts")
                                          .Where(p => p.CommunityId == communityId)
                                          .ToList();

            if (postsCache.Count > 0)
            {
                var result = new List<PostCardModel>();
                var communityName = (await communityService.GetCommunityAsync(communityId)).Name;

                foreach (var postCacheItem in postsCache)
                {
                    var arrayOfUserWhoLiked = GetUsersWhoLikedPost(postCacheItem.Id)?.UserIds;
                    var arrayOfUserWhoDisliked = GetUsersWhoDislikedPost(postCacheItem.Id)?.UserIds;

                    var postCardModel = new PostCardModel
                    {
                        Id = postCacheItem.Id,
                        Title = postCacheItem.Title,
                        CommunityName = communityName,
                        CommunityId = postCacheItem.CommunityId,
                        Date = postCacheItem.Date,
                        Text = postCacheItem.Text,
                        Likes = arrayOfUserWhoLiked.Count,
                        Dislikes = arrayOfUserWhoDisliked.Count,
                        CommentsCount = commentPostCacheService.GetCommentsCount(postCacheItem.Id),
                        UsersWhoLikedThisPost = arrayOfUserWhoLiked,
                        UsersWhoDislikedThisPost = arrayOfUserWhoDisliked
                    };

                    result.Add(postCardModel);
                }

                return result;
            }

            var postsForCommunity = await context.Posts
                                                 .Where(p => p.Community.ID == communityId)
                                                 .Select(p => new PostCardModel
                                                 {
                                                     Id = p.ID,
                                                     Title = p.Title,
                                                     Text = p.Text,
                                                     CommunityName = p.Community.Name,
                                                     CommunityId = p.Community.ID,
                                                     Date = TimeAgo(p.Date),
                                                     RawDate = p.Date
                                                 }).ToListAsync();
            
          

            var dbResult = new List<PostCardModel>();
            foreach (var postForCommunity in postsForCommunity)
            {

                var userLikedPost = GetUsersWhoLikedPost(postForCommunity.Id)?.UserIds;
                var userDislikedPost = GetUsersWhoDislikedPost(postForCommunity.Id)?.UserIds;

                postForCommunity.UsersWhoLikedThisPost = userLikedPost;
                postForCommunity.UsersWhoDislikedThisPost = userDislikedPost;

                postForCommunity.Likes = userLikedPost.Count;
                postForCommunity.Dislikes = userDislikedPost.Count;

                postForCommunity.CommentsCount = commentPostCacheService.GetCommentsCount(postForCommunity.Id);

                dbResult.Add(postForCommunity);
            

            // Store in redis
            
                var cacheItem = new PostCacheItem
                {
                    Id = postForCommunity.Id,
                    CommunityId = postForCommunity.CommunityId,
                    Date = postForCommunity.RawDate.ToString("MM/dd/yyyy h:mm tt"),
                    Text = postForCommunity.Text,
                    Title = postForCommunity.Title
                };


                cacheProvider.SetInHashSet("posts", postForCommunity.Id.ToString(), JsonSerializer.Serialize(cacheItem));
            }

            return dbResult;
        }

        public async Task<Dictionary<string, List<PostCardModel>>> GetPostsByCommuntyForUserAsync(int userId)
        {
            var result = new Dictionary<string, List<PostCardModel>>();

            var communities = (await followedCommunityServices.GetCommunitiesFollowedByUserAsync(userId)).CommunityIds;

            foreach (var community in communities)
            {
                var postsForCommunity = await GetPostsByCommunityAsync(community);

                var communityName = await communityService.GetCommunityAsync(community);

                result.Add(communityName.Name, postsForCommunity);
            }

            return result;
        }

        public async Task RemovePost(int postId)
        {
            if (postId < 0)
                throw new Exception("Id of post is invalid");

            var post = await context.Posts.FindAsync();
            context.Posts.Remove(post);
            await context.SaveChangesAsync();
        }

        public Task<List<object>> GetPostsForUser(int userId)
        {
            throw new NotImplementedException();
        }

        public LikedDislikedPostModel GetUsersWhoLikedPost(int postId)
        {
            var cacheResult = cacheProvider.GetFromHashSet<LikedDislikedPostModel>("likedPosts", postId.ToString());
            if (cacheResult != null)
            {
                return new LikedDislikedPostModel
                {
                    Id = postId,
                    UserIds = cacheResult.UserIds
                };
            }

            var usersWhoLikedPost = context.LikePosts.Where(p => p.Post.ID == postId).Select(p => p.UserID).ToList();
            var cacheModel = new LikedDislikedCommentCacheItem
            {
                Id = postId,
                UserIds = usersWhoLikedPost
            };

            cacheProvider.SetInHashSet("likedPosts", postId.ToString(), JsonSerializer.Serialize(cacheModel));

            return new LikedDislikedPostModel
            {
                Id = postId,
                UserIds = usersWhoLikedPost
            };
        }

        public LikedDislikedPostModel GetUsersWhoDislikedPost(int postId)
        {
            var cacheResult = cacheProvider.GetFromHashSet<LikedDislikedPostModel>("dislikedPosts", postId.ToString());
            if (cacheResult != null)
            {
                return new LikedDislikedPostModel
                {
                    Id = postId,
                    UserIds = cacheResult.UserIds
                };
            }

            var usersWhoDislikedPost = context.DislikePosts.Where(p => p.Post.ID == postId).Select(p => p.UserID).ToList();
            var cacheModel = new LikedDislikedCommentCacheItem
            {
                Id = postId,
                UserIds = usersWhoDislikedPost
            };

            cacheProvider.SetInHashSet("dislikedPosts", postId.ToString(), JsonSerializer.Serialize(cacheModel));

            return new LikedDislikedPostModel
            {
                Id = postId,
                UserIds = usersWhoDislikedPost
            };
        }
    }
}
