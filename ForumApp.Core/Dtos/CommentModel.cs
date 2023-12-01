using System.Collections.Generic;

namespace ForumApp.Core.Dtos
{
    public class CommentModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public int PostId { get; set; }

        public int UserId { get; set; }

        public List<int> UserWhoLikedComment { get; set; }
        public List<int> UserWhoDislikedComment { get; set; }

    }
}
