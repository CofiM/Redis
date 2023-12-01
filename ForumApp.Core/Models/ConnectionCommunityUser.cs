using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ForumApp.Core.Models
{
    public class ConnectionCommunityUser
    {
        [Key]
        public int ID { get; set; }

        public User User { get; set; }

        public Community Community { get; set; }

        public int UserID { get; set; }

        public int CommunityID { get; set; }
    }
}
