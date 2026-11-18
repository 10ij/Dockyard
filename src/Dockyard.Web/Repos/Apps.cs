using Dapper;
using Dockyard.Web.Models;
 
namespace Dockyard.Web.Data.Repositories;

public interface AppRepo
{
    Task<List<AppRecord>> GetAllAsync();
    Task<List<AppRecord>> GetVisibleForUserAsync(int userId);
    Task<AppRecord?> GetByNameAsync(string containerName);
    Task UpsertBatchAsync(List<AppRecord> apps);
    Task DeleteMissingAsync(List<string> activeNames);
}

public class AppRepository(SQLiteConn db) : AppRepo
{
    public async Task<List<AppRecord>> GetAllAsync()
    {
        using var conn = await db.OpenConnectionAsync();
        var rows = await conn.QueryAsync<AppRecord>("SELECT * FROM apps ORDER BY container_name ASC;");
        
        // Fix: Convert IEnumerable to List using Dapper's .AsList()
        // Turns out you don't need for loops, interesting
        return rows.AsList();
    }

    public async Task<List<AppRecord>> GetVisibleForUserAsync(int userId)
    {
        using var conn = await db.OpenConnectionAsync();
        const string sql = """
            SELECT a.* FROM apps a
            INNER JOIN container_permissions p ON p.container_name = a.container_name
            WHERE p.user_id = @userId AND p.can_view = 1
            ORDER BY a.container_name ASC;
            """;
        var rows = await conn.QueryAsync<AppRecord>(sql, new { userId });
        
        return rows.AsList();
    }

    public async Task<AppRecord?> GetByNameAsync(string containerName)
    {
        using var conn = await db.OpenConnectionAsync();
        return await conn.QuerySingleOrDefaultAsync<AppRecord>(
            "SELECT * FROM apps WHERE container_name = @containerName LIMIT 1;", 
            new { containerName });
    }

    public async Task UpsertBatchAsync(List<AppRecord> apps)
    {
        using var conn = await db.OpenConnectionAsync();
        using var tx = conn.BeginTransaction();

        const string sql = """
            INSERT INTO apps (container_id, container_name, image, version, status, state, ports_json, metadata_json, updated_at)
            VALUES (@ContainerId, @ContainerName, @Image, @Version, @Status, @State, @PortsJson, @MetadataJson, strftime('%Y-%m-%dT%H:%M:%fZ', 'now'))
            ON CONFLICT(container_name) DO UPDATE SET
                container_id = excluded.container_id,
                image = excluded.image,
                version = excluded.version,
                status = excluded.status,
                state = excluded.state,
                ports_json = excluded.ports_json,
                metadata_json = excluded.metadata_json,
                updated_at = excluded.updated_at;
            """;

        await conn.ExecuteAsync(sql, apps, tx);
        tx.Commit();
    }

    public async Task DeleteMissingAsync(List<string> activeNames)
    {
        var names = activeNames.ToArray();
        using var conn = await db.OpenConnectionAsync();
        
        if (names.Length == 0)
        {
            await conn.ExecuteAsync("DELETE FROM apps;");
            return;
        }

        await conn.ExecuteAsync("DELETE FROM apps WHERE container_name NOT IN @names;", new { names });
    }
}