using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumApp.Core.Dtos
{
    public class FollowedCommunityModel
    {
        public int UserId { get; set; }

        public List<int> CommunityIds { get; set; }
    }
}
