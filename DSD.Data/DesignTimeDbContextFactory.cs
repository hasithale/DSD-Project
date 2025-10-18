using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DSD.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            // Use localdb for dev by default. Change if you use SQL Server elsewhere.
            optionsBuilder.UseSqlServer("Server=localhost;Database=DSDRoute;User Id=hasithe;Password=Hazz119;MultipleActiveResultSets=True;TrustServerCertificate=True;");
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
