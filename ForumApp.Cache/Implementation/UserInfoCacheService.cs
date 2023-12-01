using ForumApp.Cache.Classes;
using ForumApp.Cache.Interfaces;
using ForumApp.Core.Context;
using ForumApp.Core.Interfaces.CacheServices;
using Newtonsoft.Json;
using System.Linq;

namespace ForumApp.Cache.Implementation
{
    public class UserInfoCacheService : IUserInfoCacheService
    {
        private ForumContext context { get; set; }

        private ICacheProvider cacheProvider { get; set; }

        public UserInfoCacheService(ForumContext context, ICacheProvider cacheProvider)
        {
            this.context = context;
            this.cacheProvider = cacheProvider;
        }

        public void BuildOrRefreshCache()
        {
            var users = context.Users.ToList();

            foreach (var user in users)
            {
                var cacheItem = new UserInfoCacheItem
                {
                    Id = user.ID,
                    Username = user.Username,
                    Mail = user.Mail
                };

                cacheProvider.SetInHashSet("users", user.ID.ToString(), JsonConvert.SerializeObject(cacheItem));
            }
        }

        public UserInfoCacheItem GetUser(int userId)
        {
            return cacheProvider.GetFromHashSet<UserInfoCacheItem>("users", userId.ToString());
        }
    }
}
