using ForumApp.Cache.Classes;
using ForumApp.Core.Context;
using ForumApp.Core.Interfaces.CacheServices;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace ForumApp.Cache.Implementation
{
    public class CommentPostCacheService : ICommentPostCacheService
    {
        private ForumContext context { get; set; }
        
        private ICacheProvider cacheProvider { get; set; }

        public CommentPostCacheService(ForumContext context, ICacheProvider cacheProvider)
        {
            this.context = context;
            this.cacheProvider = cacheProvider;
        }

        public void BuildOrRefreshCache()
        {
            var commentsByPostId = context.Comments.Select(p => new { PostId = p.PostID, CommentId = p.ID  })
                                       .ToList()
                                       .GroupBy(p => p.PostId, (key, value) => new { PostId = key, CommentIds = value.Select(p => p.CommentId) })
                                       .ToList();

            foreach (var commentByPostId in commentsByPostId)
            {
                var cacheItem = new CommentPostCacheItem
                {
                    Id = commentByPostId.PostId,
                    CommentIds = commentByPostId.CommentIds
                };

                cacheProvider.SetInHashSet("commentsPost", commentByPostId.PostId.ToString(), JsonConvert.SerializeObject(cacheItem));
            }
        }

        public CommentPostCacheItem GetAndCacheCommentsCount(int postId)
        {
            var postComments = context.Comments.Where(item => item.PostID == postId).Select(item => item.ID).ToList();
            var cacheItem = new CommentPostCacheItem
            {
                Id = postId,
                CommentIds = postComments
            };

            cacheProvider.SetInHashSet("commentsPost", postId.ToString(), JsonConvert.SerializeObject(cacheItem));

            return cacheItem;
        }

        public int GetCommentsCount(int postId)
        {
            var cacheData = cacheProvider.GetFromHashSet<CommentPostCacheItem>("commentsPost", postId.ToString());
            if (cacheData != null)
            {
                return cacheData.CommentIds.Count();
            }

            return GetAndCacheCommentsCount(postId).CommentIds.Count();
        }
    }
}
