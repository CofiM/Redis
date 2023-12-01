using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace ForumApp.Core.Models
{
    public class User
    {
        [Key]
        public int ID { get; set; }

        public string Username { get; set; }

        public string Mail { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        //fk

        [JsonIgnore]
        public List<Post> Posts { get; set; }
        [JsonIgnore]
        public List<ConnectionCommunityUser> Communities { get; set; }

        [JsonIgnore]
        public List<Comment> Comments { get; set; }

        [JsonIgnore]
        public List<LikePost> LikePosts { get; set; }

        [JsonIgnore]
        public List<DislikePost> DislikePosts { get; set; }

        [JsonIgnore]
        public List<LikeComment> LikeComments { get; set; }

        [JsonIgnore]
        public List<DislikeComment> DislikeComments { get; set; }
    }
}
