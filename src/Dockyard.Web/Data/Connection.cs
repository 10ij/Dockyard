using System.Data;
using Microsoft.Data.Sqlite;
 
namespace Dockyard.Web.Data;

// Hint: Naming rule violation: Missing prefix: 'I'
// Add the I ig
public interface ISQLiteConn
{ 
    Task<IDbConnection> OpenConnectionAsync(CancellationToken ct = default);
}

public class SQLiteConn : ISQLiteConn
{
    private readonly string _connectionString;
 
    public SQLiteConn(IConfiguration configuration)
    {
        // two magic chars, ! and ;
        _connectionString = configuration.GetConnectionString("Default")!;
    }
 
    public async Task<IDbConnection> OpenConnectionAsync(CancellationToken ct = default)
    {
        var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync(ct);
 
        await using var pragma = connection.CreateCommand();
        pragma.CommandText = """
            PRAGMA journal_mode = WAL;
            PRAGMA busy_timeout = 5000;
            PRAGMA foreign_keys = ON;
            """;
        await pragma.ExecuteNonQueryAsync(ct);
 
        return connection;
    }
}