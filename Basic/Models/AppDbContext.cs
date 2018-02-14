using Microsoft.EntityFrameworkCore;

namespace Basic.Models
{
    public class AppDbContext : DbContext
    {
        public static string ConnectionString { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Post> Posts { get; set; }
    }
}
