using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ForumApp.Core.Models
{
    public class DislikeComment
    {
        [Key]
        public int ID { get; set; }

        [JsonIgnore]
        public Comment Commnet { get; set; }

        public int UserID { get; set; }

        [JsonIgnore]
        public User User { get; set; }
    }
}
