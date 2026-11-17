using System.Reflection;
// using DbUp;
using Microsoft.Data.Sqlite;

namespace Dockyard.Web.Data;

public static class DatabaseMigrator
{
    public static void EnsureMigrated(string connectionString)
    {
        var csBuilder = new SqliteConnectionStringBuilder(connectionString);
        var dbDirectory = Path.GetDirectoryName(csBuilder.DataSource);
        if (!string.IsNullOrWhiteSpace(dbDirectory))
        {
            Directory.CreateDirectory(dbDirectory);
        }

        // Enable WAL mode once before running migrations
        using (var conn = new SqliteConnection(connectionString))
        {
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "PRAGMA journal_mode = WAL; PRAGMA foreign_keys = ON;";
            cmd.ExecuteNonQuery();
        }

        var upgrader = DbUp.DeployChanges.To
            .SqliteDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
            .WithTransactionPerScript()
            .LogToConsole()
            .Build();

        var result = upgrader.PerformUpgrade();
        if (!result.Successful)
        {
            throw new InvalidOperationException(
                "Database migration failed.", result.Error);
        }
    }
}