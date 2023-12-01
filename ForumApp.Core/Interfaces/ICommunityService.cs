using ForumApp.Core.Dtos;
using ForumApp.Core.Interfaces.CacheServices;
using ForumApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumApp.Core.Interfaces
{
    public interface ICommunityService : ICacheServiceBase
    {
        Task AddCommunityAsync(string name);

        Task FollowCommunityAsync(int userId, int communityId);

        Task UnfollowCommunityAsync(int userId, int communityId);

        Task<List<Post>> GetAllPostForCommunity(int communityId);

        Task<List<ConnectionCommunityUser>> GetAllAccountWhichFollowCommunity(int communityId);

        Task<List<CommunityModel>> GetAllCommunities();

        Task<CommunityModel> GetCommunityAsync(int id);
    }
}
