using forum.business.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace forum.business.Data
{
    public class ForumDbContext : IdentityDbContext
    {
        public DbSet<Discussion> Discussions { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PictureOnDatabaseModel> PicturesOnDatabase { get; set; }
        public DbSet<PictureOnFileSystemModel> PicturesOnDatabase { get; set; }
        public ForumDbContext(DbContextOptions<ForumDbContext> options) : base(options) { }
    }
}
