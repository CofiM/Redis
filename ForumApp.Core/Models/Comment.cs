using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ForumApp.Core.Models
{
    public class Comment
    {
        [Key]
        public int ID { get; set; }
        
        public string Text { get; set; }

        public int PostID { get; set; }

        [JsonIgnore]
        public List<LikeComment> LikeComments { get; set; }

        [JsonIgnore]
        public List<DislikeComment> DislikeComments { get; set; }

        [JsonIgnore]
        public User User { get; set; }

        [JsonIgnore]
        public Post Post { get; set; }
    }
}
