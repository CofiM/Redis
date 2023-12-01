using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumApp.Core.Dtos
{
    public class LikedDislikedCommentModel
    {
        public int Id { get; set; }

        public List<int> UserIds { get; set; }
    }
}
