using ForumApp.Cache;
using ForumApp.Cache.Classes;
using ForumApp.Core.Context;
using ForumApp.Core.Dtos;
using ForumApp.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ForumApp.Services.Implementations
{
    public class FollowedCommunityService : IFollowedCommunityServices
    {
        private ForumContext context { get; set; }

        private ICacheProvider cacheProvider { get; set; }

        public FollowedCommunityService(ForumContext context, ICacheProvider cache)
        {
            this.context = context;
            cacheProvider = cache;
        }

        public async Task<FollowedCommunityModel> GetCommunitiesFollowedByUserAsync(int userId)
        {
            var cacheResult = cacheProvider.GetFromHashSet<FollowedCommunityCacheItem>("communitiesFollowedByUser", userId.ToString());

            if(cacheResult != null)
            {
                var model = new FollowedCommunityModel
                {
                    UserId = userId,
                    CommunityIds = cacheResult.CommunityIds.ToList()
                };

                return model;
            }

            var dbResult = await context.Connections
                            .Where(p => p.UserID == userId).ToListAsync();

            var communityIDs = dbResult.Select(p => p.CommunityID).ToList();

            var followedCommunityModel = new FollowedCommunityModel
            {
                UserId = userId,
                CommunityIds = communityIDs
            };

            var followedCommunityCache = new FollowedCommunityCacheItem
            {
                UserId = userId,
                CommunityIds = communityIDs
            };

            cacheProvider.SetInHashSet("communitiesFollowedByUser", userId.ToString(), JsonSerializer.Serialize(followedCommunityCache));

            return followedCommunityModel;

        }
    }
}
