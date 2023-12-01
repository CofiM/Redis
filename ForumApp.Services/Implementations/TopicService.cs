using ForumApp.Core.Context;
using ForumApp.Core.Interfaces;
using ForumApp.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumApp.Services.Implementations
{
    public class TopicService : ITopicService
    {
        private ForumContext context { get; set; }

        public TopicService(ForumContext context)
        {
            this.context = context;
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
        
        public async Task CreateTopicAsync(string name)
        {
           if(string.IsNullOrWhiteSpace(name))
            {
                throw new Exception("Provided name is null!");
            }
            var topic = await context.Topics.Where(p => p.Name == name).FirstOrDefaultAsync();

            if (topic != null)
            {
                throw new Exception("Topic with this name exist!");
            }

            Topic newTopic = new Topic();
            newTopic.Name = name;

            context.Topics.Add(newTopic);
            await context.SaveChangesAsync();
        }

        public async Task<List<Topic>> GetAllTopicsAsync()
        {
            return await context.Topics.ToListAsync();
        }

        public async Task<List<Post>> GetPostsForTopicAsync(int topicId)
        {
            if (topicId < 0)
                throw new Exception("Bad id for topic!");

            var posts = await context.Posts.Include(p => p.Topic)
                                            .Where(p => p.Topic.ID == topicId)
                                            .ToListAsync();
            if (posts == null)
                throw new Exception("There aren't any posts for this topic!");

            return posts;
        }

        public async Task<List<object>> GetPostForTopicStringAsync(string name, int userId)
        {
            var listOfObject = new List<object>();

            var posts = await context.Posts
                .Include(p => p.Community)
                .Include(p => p.Likes)
                .Include(p => p.Dislikes)
                .Include(p => p.Comments)
                .ThenInclude(p => p.LikeComments)
                .Include(p => p.Comments)
                .ThenInclude(p => p.DislikeComments)
                .Where(p => p.Topic.Name == name)
                .Select(p => new
                {
                    p.ID,
                    p.Title,
                    p.Text,
                    Likes = p.Likes.Count,
                    Dislikes = p.Dislikes.Count,
                    Community = p.Community.Name,
                    Date = TimeAgo(p.Date),
                    Comments = p.Comments.Select(q => new
                    {
                        IDComment = q.ID,
                        q.Text,
                        LikesComment = q.LikeComments.Count,
                        DislikeComment = q.DislikeComments.Count
                    })

                })
                .ToListAsync();
            if (posts == null)
                throw new Exception("There aren't any posts for this topic!");

            foreach(var post in posts)
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

                listOfObject.Add(term);
            }

            return listOfObject;
        }
    }
}
