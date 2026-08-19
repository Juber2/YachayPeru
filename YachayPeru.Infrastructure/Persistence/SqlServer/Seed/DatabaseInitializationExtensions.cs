using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YachayPeru.Infrastructure.Persistence.SqlServer;
using System;
using System.Collections.Generic;
using System.Text;

namespace YachayPeru.Infrastructure.Persistence.SqlServer.Seed
{
    public static class DatabaseInitializationExtensions
    {
        public static async Task InitialiseDatabaseAsync(this IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var seeder = scope.ServiceProvider.GetRequiredService<DbSeeder>();

            try
            {
                logger.LogInformation("Applying database migrations...");
                await context.Database.MigrateAsync();

                logger.LogInformation("Running database seed...");
                await seeder.SeedAsync();

                logger.LogInformation("Database initialization completed.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while initializing the database.");
                throw;
            }
        }
    }
}
