using ForumApp.Core.Dtos;
using ForumApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumApp.Core.Interfaces
{
    public interface IPostLikeService
    {
        Task<bool> IsPostAlredyLikedAsync(int userId, int postId);

        Task<bool> IsPostAlredyDisikedAsync(int userId, int postId);

        Task<int> RemoveLikeAsync(int userId, int postId);

        Task<int> AddLikePostAsync(int userId, int postId);

        Task<object> LikePostAsync(int userId, int postId);

        Task<int> AddDislikePostAsync(int userId, int postId);

        Task<object> DislikePostAsync(int userId, int postId);

        Task<int> RemoveDislikeAsync(int userId, int postId);

        Task<PostReactionsModel> GetPostReactionsAsync(int postId);

        List<LikedDislikedPostModel> GetUsersWhoLikedPost(int postId);

        List<LikedDislikedPostModel> GetUsersWhoDislikedPost(int postId);
    }
}
