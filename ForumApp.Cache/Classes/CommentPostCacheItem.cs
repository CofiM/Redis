using System.Collections.Generic;

namespace ForumApp.Cache.Classes
{
    public class CommentPostCacheItem
    {
        public int Id { get; set; }

        public IEnumerable<int> CommentIds { get; set; }
    }
}
