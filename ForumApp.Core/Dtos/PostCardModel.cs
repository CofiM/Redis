using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumApp.Core.Dtos
{
    public class PostCardModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public int Likes { get; set; }

        public int Dislikes { get; set; }

        public string CommunityName { get; set; }

        public int CommunityId { get; set; }

        public string Date { get; set; }

        public int CommentsCount { get; set; }

        public List<int> UsersWhoLikedThisPost { get; set; }

        public List<int> UsersWhoDislikedThisPost { get; set; }

        public DateTime RawDate { get; set; }

    }
}
