using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumApp.Cache.Classes
{
    public class FollowedCommunityCacheItem
    {
        public int UserId { get; set; }

        public List<int> CommunityIds { get; set; }
    }
}
