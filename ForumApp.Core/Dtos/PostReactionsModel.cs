using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumApp.Core.Dtos
{
    public class PostReactionsModel
    {
        public int Id { get; set; }

        public int LikesCount { get; set; }

        public int DislikesCount { get; set; }
    }
}
