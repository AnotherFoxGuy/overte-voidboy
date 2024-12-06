using Microsoft.Extensions.Logging;

namespace DirectoryService.DAL.Infrastructure;

public static class DatabaseMigrator
{
    /// <summary>
    /// Database Migrator
    /// </summary>
    public static bool MigrateDatabase(ILogger logger)
    {
        using var con = new DbContext().CreateConnection();

        logger.LogInformation("Beginning Migrations");

        return true;
    }
}