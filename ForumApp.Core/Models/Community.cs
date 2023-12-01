using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ForumApp.Core.Models
{
    public class Community
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        //fk
        [JsonIgnore]
        public List<Post> Posts { get; set; }

        [JsonIgnore]
        public List<ConnectionCommunityUser> Users { get; set; }
    }
}
