using ForumApp.Cache;
using ForumApp.Cache.Classes;
using ForumApp.Core.Context;
using ForumApp.Core.Dtos;
using ForumApp.Core.Interfaces;
using ForumApp.Core.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumApp.Services.Implementations
{
    public class CommunityService : ICommunityService
    {
        private ForumContext context { get; set; }

        private ICacheProvider cacheProvider { get; set; }

        public CommunityService(ForumContext Context, ICacheProvider cache)
        {
            this.context = Context;
            cacheProvider = cache;
        }

        public void BuildOrRefreshCache()
        {
            var communities = context.Communities.ToList();
            foreach (var community in communities)
            {
                var cacheItem = new CommunityCacheItem
                {
                    Id = community.ID,
                    Name = community.Name
                };

                cacheProvider.SetInHashSet("communities", community.ID.ToString(), JsonConvert.SerializeObject(cacheItem));
            }
        }

       
        private async Task<Community> GetCommunityAsync(string name)
        {
            return await context.Communities.Where(p => p.Name == name).FirstOrDefaultAsync();
        }

        
        private async Task<bool> IsCommunityAlreadyExist(string name)
        {
            var community = await GetCommunityAsync(name);
            if (community == null)
            {
                return false;
            }
            return true;
        }

        private async Task<User> GetUserAsync(int userId)
        {
            return await context.Users.FirstOrDefaultAsync(p => p.ID == userId);
        }

        private async Task<Community> GetCommunityIdAsync(int communityId)
        {
            return await context.Communities.FirstOrDefaultAsync(p => p.ID == communityId);
        }

        private async Task<ConnectionCommunityUser> GetConnectionAsync(int userId, int communityId)
        {
            return await context.Connections.FirstOrDefaultAsync(p => p.User.ID == userId && p.Community.ID == communityId);
        }

        public async Task AddCommunityAsync(string name)
        {
            if(await IsCommunityAlreadyExist(name))
            {
                throw new InvalidOperationException("Community with this name already exist!");
            }

            var community = new Community();
            community.Name = name;

            context.Communities.Add(community);
            await context.SaveChangesAsync();

            var dbResult = await context.Communities.ToListAsync();
            foreach(var item in dbResult)
            {
                var cache = new CommunityCacheItem
                {
                    Id = item.ID,
                    Name = item.Name
                };

                cacheProvider.SetInHashSet("communities", item.ID.ToString(), JsonConvert.SerializeObject(cache));
            }
        }

        public async Task FollowCommunityAsync(int userId, int communityId)
        {
            if(context.Connections.FirstOrDefault(p => p.User.ID == userId && p.Community.ID == communityId) != null)
            {
                throw new InvalidOperationException("User already follow community!");
            }

            var user = await GetUserAsync(userId);
            var community = await GetCommunityIdAsync(communityId);

            if(user == null || community == null)
            {
                throw new InvalidOperationException("User or community does not exist.");
            }

            var connect = new ConnectionCommunityUser
            {
                User = user,
                Community = community
            };

            context.Connections.Add(connect);
            await context.SaveChangesAsync();

            var communitiesFollowedByUser = context.Connections.Where(p => p.UserID == userId).Select(p => p.CommunityID).ToList();
            var cacheItem = new FollowedCommunityCacheItem
            {
                UserId = userId,
                CommunityIds = communitiesFollowedByUser
            };

            cacheProvider.SetInHashSet("communitiesFollowedByUser", userId.ToString(), JsonConvert.SerializeObject(cacheItem));
        }

        public async Task UnfollowCommunityAsync(int userId, int communityId)
        {
            var connection = await GetConnectionAsync(userId, communityId);
            
            if(connection == null)
            {
                throw new InvalidOperationException("User not follow community!");
            }

            context.Connections.Remove(connection);
            await context.SaveChangesAsync();
        }

        public async Task<List<Post>> GetAllPostForCommunity(int communityId)
        {

            var posts = await context.Posts
                     .Include(p => p.Community)
                     .Where(p => p.Community.ID == communityId)
                     .ToListAsync();

            return posts;
        }

        public async Task<List<ConnectionCommunityUser>> GetAllAccountWhichFollowCommunity(int communityId)
        {
            var connections = await context.Connections
                            .Include(p => p.User)
                            .Include(p => p.Community)
                            .Where(p => p.Community.ID == communityId)
                            .ToListAsync();
            return connections;
        }
    
        public async Task<List<CommunityModel>> GetAllCommunities()
        {
            var cacheResult = cacheProvider.GetAllFromHashSet<CommunityCacheItem>("communities");

            if (cacheResult.Any())
            {
                var communityModel = new List<CommunityModel>();
                foreach(var item in cacheResult)
                {
                    var model = new CommunityModel
                    {
                        Id = item.Id,
                        Name = item.Name
                    };
                    communityModel.Add(model);
                }
                return communityModel;
            }


            var dbResult = await context.Communities.ToListAsync();
            var communitiesModel = new List<CommunityModel>();
            var communitiesCache = new List<CommunityCacheItem>();

            foreach(var item in dbResult)
            {
                var model = new CommunityModel
                {
                    Id = item.ID,
                    Name = item.Name
                };

                communitiesModel.Add(model);

                var cache = new CommunityCacheItem
                {
                    Id = item.ID,
                    Name = item.Name
                };

                communitiesCache.Add(cache);
            }

            foreach(var res in communitiesCache)
            {
                cacheProvider.SetInHashSet("communities", res.Id.ToString(), JsonConvert.SerializeObject(res));
            }

            return communitiesModel;
        }

        public async Task<CommunityModel> GetCommunityAsync(int id)
        {
            var communityCache = cacheProvider.GetFromHashSet<CommunityCacheItem>("communities", id.ToString());

            if(communityCache != null)
            {
                return new CommunityModel { Id = communityCache.Id, Name = communityCache.Name };
            }

            var communityDb = context.Communities.FirstOrDefault(p => p.ID == id);

            if (communityDb == null)
            {
                throw new InvalidOperationException("Communities don't exist in database.");
            }

            cacheProvider.SetInHashSet("communities", id.ToString(), JsonConvert.SerializeObject(new CommunityCacheItem
            {
                Id = communityDb.ID,
                Name = communityDb.Name
            }));

            return new CommunityModel { Id = communityDb.ID, Name = communityDb.Name };

        }

    }
}
