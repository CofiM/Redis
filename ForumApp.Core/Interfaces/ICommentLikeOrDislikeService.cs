using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumApp.Core.Interfaces
{
    public interface ICommentLikeOrDislikeService
    {
        Task AddDislikeCommentAsync(int userId, int commentId);

        Task RemoveDislikeCommentAsync(int userId, int commentId);

        Task DislikeCommentAsync(int userId, int commentId);

        Task AddLikeCommentAsync(int userId, int commentId);

        Task RemoveLikeCommentAsync(int userId, int commentId);

        Task LikeCommentAsync(int userId, int commentId);

        Task<bool> IsCommentAlreadyLikeAsync(int userId, int commentId);

    }
}
