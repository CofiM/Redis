using ForumApp.Core.Dtos;
using ForumApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumApp.Core.Interfaces
{
    public interface IPostService
    {
        Task AddPost(string title, string text, string topic, int userId, int communityId);

        Task RemovePost(int idPost);

        Task<List<object>> GetPostsForCommunity(int communityId, int userId);

        Task<List<object>> GetPostsForUser(int userId);

        Task<List<Post>> GetPostsForTopic(int topicId);

        Task<Dictionary<string, List<PostCardModel>>> GetPostsByCommuntyForUserAsync(int userId);

        LikedDislikedPostModel GetUsersWhoLikedPost(int postId);

        LikedDislikedPostModel GetUsersWhoDislikedPost(int postId);
    }
}
