using ForumApp.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumApp.Core.Interfaces
{
    public interface IFollowedCommunityServices
    {
        Task<FollowedCommunityModel> GetCommunitiesFollowedByUserAsync(int userId);
    }
}
