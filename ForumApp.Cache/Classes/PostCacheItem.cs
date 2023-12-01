using ForumApp.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumApp.Cache.Classes
{
    public class PostCacheItem
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public string Date { get; set; }

        public int CommunityId { get; set; }

    }
}
