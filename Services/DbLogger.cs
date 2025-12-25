using Microsoft.Data.SqlClient;

namespace ITHelpDesk.Services;

public interface IDbLogger
{
    Task LogAsync(string tableName, string operation, int recordId, string? oldData, string? newData);
}

public class DbLogger(IConfiguration config) : IDbLogger
{
    public async Task LogAsync(string tableName, string operation, int recordId, string? oldData, string? newData)
    {
        var connStr = config.GetConnectionString("DefaultConnection");
        using var conn = new SqlConnection(connStr);
        await conn.OpenAsync();

        var sql = "INSERT INTO Logs_Table (TableName, Operation, RecordId, OldData, NewData, UserId) VALUES (@t, @o, @r, @old, @new, @u)";
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@t", tableName);
        cmd.Parameters.AddWithValue("@o", operation);
        cmd.Parameters.AddWithValue("@r", recordId);
        cmd.Parameters.AddWithValue("@old", (object?)oldData ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@new", (object?)newData ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@u", "System");

        await cmd.ExecuteNonQueryAsync();
    }
}