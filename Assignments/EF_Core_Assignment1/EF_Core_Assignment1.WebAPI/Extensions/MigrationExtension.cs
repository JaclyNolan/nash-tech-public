using EF_Core_Assignment1.Persistance.Contexts;
using EF_Core_Assignment1.Persistance.Seeder;
using Microsoft.EntityFrameworkCore;

namespace EF_Core_Assignment1.WebAPI.Extensions
{
    public static class MigrationExtension
    {
        public static IHost MigrationDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<NashTechContext>();
                dbContext.Database.Migrate();
            }

            return host;
        }

        public static IHost SeedDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<NashTechContext>();
                    NashTechSeeder.SeederAsync(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
            return host;
        }
    }
}
