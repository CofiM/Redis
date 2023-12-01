using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ForumApp.Core.Models
{
    public class DislikePost
    {

        [Key]
        public int ID { get; set; }

        public int UserID { get; set; }

        [JsonIgnore]
        public User User { get; set; }

        [JsonIgnore]
        public Post Post { get; set; }
    }
}
