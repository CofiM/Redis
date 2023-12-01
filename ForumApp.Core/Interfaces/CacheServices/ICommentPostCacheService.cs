namespace ForumApp.Core.Interfaces.CacheServices
{
    public interface ICommentPostCacheService : ICacheServiceBase
    {
        int GetCommentsCount(int postId);
    }
}
