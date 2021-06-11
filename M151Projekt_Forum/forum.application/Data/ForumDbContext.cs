using forum.business.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace forum.application.Data
{
    public class ForumDbContext : IdentityDbContext
    {
        public DbSet<Discussion> Discussions { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public ForumDbContext(DbContextOptions<ForumDbContext> options) : base(options) { }
    }
}
