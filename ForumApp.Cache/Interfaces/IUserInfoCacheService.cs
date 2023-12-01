using ForumApp.Cache.Classes;
using ForumApp.Core.Interfaces.CacheServices;

namespace ForumApp.Cache.Interfaces
{
    public interface IUserInfoCacheService : ICacheServiceBase
    {
        UserInfoCacheItem GetUser(int userId);
    }
}
