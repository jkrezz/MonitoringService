using Dapper;
using WebApi.Models;

namespace WebApi.Data;

/// <summary>
/// Инициализатор схемы базы данных и тестовых данных.
/// </summary>
public class DbInitializer
{
    private readonly DapperContext _context;
    private readonly ILogger<DbInitializer> _logger;

    /// <summary>
    /// Инициализирует новый экземпляр инициализатора базы данных.
    /// </summary>
    /// <param name="context">Контекст Dapper для работы с БД.</param>
    /// <param name="logger">Логгер.</param>
    public DbInitializer(DapperContext context, ILogger<DbInitializer> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Применяет схему базы данных и при необходимости наполняет её начальными данными.
    /// </summary>
    public async Task InitializeAsync()
    {
        const string sql = """
        CREATE TABLE IF NOT EXISTS devices (
            id UUID PRIMARY KEY,
            name TEXT NOT NULL
        );

        CREATE TABLE IF NOT EXISTS sessions (
            id UUID PRIMARY KEY,
            device_id UUID NOT NULL REFERENCES devices(id) ON DELETE CASCADE,
            start_time TIMESTAMPTZ NOT NULL,
            end_time TIMESTAMPTZ NOT NULL,
            version TEXT NOT NULL
        );
        """;

        using var connection = _context.CreateConnection();
        _logger.LogInformation("Применение схемы базы данных при её отсутствии");
        await connection.ExecuteAsync(sql);

        var existingCount = await connection.QuerySingleAsync<long>("SELECT COUNT(*) FROM devices;");
        if (existingCount == 0)
        {
            const string insertSql = """
            INSERT INTO devices (id, name)
            VALUES (@Id, @Name);
            """;

            var devices = new[]
            {
                new Device { Id = Guid.NewGuid(), Name = "John Doe" },
                new Device { Id = Guid.NewGuid(), Name = "Jack Smith" },
                new Device { Id = Guid.NewGuid(), Name = "Lewis Carroll" }
            };

            await connection.ExecuteAsync(insertSql, devices);
        }
    }
}