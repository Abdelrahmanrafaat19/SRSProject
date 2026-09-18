using Microsoft.EntityFrameworkCore;
using SRSProject.Infrastructure.DataContext;

namespace SRSProject.Infrastructure.Extentions
{
    public static class ProgramExtension
    {
        public static async Task MigrationAndSeedAsync(
            this WebApplication app,
            CancellationToken cancellationToken = default)
        {
            await using var scope =
                app.Services.CreateAsyncScope();

            var dbContext =
                scope.ServiceProvider
                    .GetRequiredService<SRSDbContext>();



            var logger =
                scope.ServiceProvider
                    .GetRequiredService<ILogger<Program>>();

            try
            {
                var pendingMigrations =
                    (await dbContext.Database
                        .GetPendingMigrationsAsync(cancellationToken))
                    .ToList();

                if (pendingMigrations.Any())
                {
                    logger.LogInformation(
                        "Applying {Count} pending migrations...",
                        pendingMigrations.Count);

                    await dbContext.Database
                        .MigrateAsync(cancellationToken);

                    logger.LogInformation(
                        "Migrations applied successfully.");
                }
                else
                {
                    logger.LogInformation(
                        "No pending migrations found.");
                }




            }
            catch (Exception exception)
            {
                logger.LogCritical(
                    exception,
                    "An error occurred while migrating or seeding the database.");

                throw;
            }
        }
    }
}
