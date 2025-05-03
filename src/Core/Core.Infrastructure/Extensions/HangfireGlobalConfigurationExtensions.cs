using Hangfire.PostgreSql;
using Hangfire;
using Npgsql;

namespace Core.Infrastructure.Extensions;

internal static class HangfireGlobalConfigurationExtensions
{
    public static IGlobalConfiguration UsePostgreSqlStorage(
       this IGlobalConfiguration configuration,
       string connectionString,
       PostgreSqlStorageOptions? storageOptions = null)
    {
        var databaseName = GetDatabaseNameFromConnectionString(connectionString);

        var builder = new NpgsqlConnectionStringBuilder(connectionString) { Database = "postgres" };

        using var connection = new NpgsqlConnection(builder.ConnectionString);
        connection.Open();

        var exists = IsExistingDatabase(connection, databaseName);
        if (!exists)
        {
            CreateDatabase(connection, databaseName);
        }

        return configuration.UsePostgreSqlStorage(x => x.UseNpgsqlConnection(connectionString), storageOptions ?? new PostgreSqlStorageOptions());
    }

    private static string GetDatabaseNameFromConnectionString(string connectionString)
    {
        return connectionString
            .Split(';')
            .FirstOrDefault(x => x.StartsWith("Database=", StringComparison.OrdinalIgnoreCase))
            ?.Split('=')[1] ?? throw new ArgumentException("There no provided database in connection string");
    }

    private static bool IsExistingDatabase(NpgsqlConnection connection, string databaseName)
    {
        const string query = "SELECT 1 FROM pg_database WHERE datname = @databaseName";
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("databaseName", databaseName);

        return command.ExecuteScalar() != null;
    }

    private static void CreateDatabase(NpgsqlConnection connection, string databaseName)
    {
        var createDbQuery = $"CREATE DATABASE \"{databaseName}\"";
        using var createDbCommand = new NpgsqlCommand(createDbQuery, connection);
        createDbCommand.ExecuteNonQuery();
    }
}
