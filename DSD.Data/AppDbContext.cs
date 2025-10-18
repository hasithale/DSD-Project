using Microsoft.EntityFrameworkCore;

namespace DSD.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Add DbSets later. For now keep empty to allow migrations.
        // public DbSet<User> Users { get; set; }
    }
}
