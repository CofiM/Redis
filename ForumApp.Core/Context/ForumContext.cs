using ForumApp.Core.Models;
using Microsoft.EntityFrameworkCore;


namespace ForumApp.Core.Context
{
    public class ForumContext : DbContext
    {
        public DbSet<Comment> Comments { get; set; }

        public DbSet<Community> Communities { get; set; }

        public DbSet<DislikeComment> DislikeComments { get; set; }

        public DbSet<DislikePost> DislikePosts { get; set; }

        public DbSet<LikeComment> LikeComments { get; set; }

        public DbSet<LikePost> LikePosts { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Topic> Topics { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<ConnectionCommunityUser> Connections { get; set; }

        public ForumContext(DbContextOptions opt) : base(opt)
        {

        }
    }
}
