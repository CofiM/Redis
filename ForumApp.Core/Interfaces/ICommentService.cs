using ForumApp.Core.Dtos;
using ForumApp.Core.Interfaces.CacheServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForumApp.Core.Interfaces
{
    public interface ICommentService
    {
        Task AddCommentAsync(int userId, int postId, string textComment);

        Task RemoveCommentAsync(int userId, int commentId);

        List<CommentModel> GetCommentsForPost(int postId);

        List<LikedDislikedCommentModel> GetUsersWhoLikedComment(int commentId);

        List<LikedDislikedCommentModel> GetUsersWhoDislikeComment(int commentId);
    }
}
