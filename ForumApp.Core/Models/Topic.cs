using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ForumApp.Core.Models
{
    public class Topic
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public List<Post> Posts { get; set; } 
    }
}
