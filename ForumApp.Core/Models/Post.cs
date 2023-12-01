using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ForumApp.Core.Models
{
    public class Post
    {
        [Key]
        public int ID { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public DateTime Date { get; set; } 


        [JsonIgnore]
        public List<Comment> Comments { get; set; }

        [JsonIgnore]
        public List<LikePost> Likes { get; set; }

        [JsonIgnore]
        public List<DislikePost> Dislikes { get; set; }

        [JsonIgnore]
        public Topic Topic { get; set; }

        [JsonIgnore]
        public User User { get; set; }

        [JsonIgnore]
        public Community Community { get; set; }
    }
}
