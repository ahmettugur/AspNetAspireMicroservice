using Microsoft.EntityFrameworkCore;

namespace Clients.Api.Data;

public static class DataExtensions
{
    public static async Task InitializeDbAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ClientsDbContext>();
        var connString = dbContext.Database.GetConnectionString();

        var logger = serviceProvider.GetRequiredService<ILoggerFactory>()
                                    .CreateLogger("DB Initializer");

        var strategy = dbContext.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(() => dbContext.Database.MigrateAsync());

        logger.LogInformation(5, "The database is ready!");
    }
}