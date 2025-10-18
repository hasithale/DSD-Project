using DSD.Data;
using DSD.Core.Entities;
using DSD.API.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace DSD.API
{
    public static class StartupSeed
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

            // Ensure migrations applied (for dev)
            await db.Database.MigrateAsync();

            var existing = await db.Users.FirstOrDefaultAsync(u => u.Username == "superadmin");
            if (existing == null)
            {
                var user = new User
                {
                    Username = "superadmin",
                    FullName = "Super Admin",
                    Email = "admin@example.com",
                    Role = "SuperAdmin",
                    IsActive = true,
                    PasswordHash = hasher.Hash("Password@123") // change later
                };
                db.Users.Add(user);
                await db.SaveChangesAsync();
            }
        }
    }
}
