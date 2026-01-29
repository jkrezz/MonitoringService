using System.Data;
using Npgsql;

namespace WebApi.Data;

/// <summary>
/// Контекст для работы с базой данных через Dapper.
/// </summary>
/// <remarks>
/// Отвечает за создание подключений к Postgres с использованием строки подключения из конфигурации.
/// </remarks>
public class DapperContext
{
    private readonly IConfiguration _config;
    private readonly string _connectionString;
    private readonly ILogger<DapperContext> _logger;

    /// <summary>
    /// Инициализирует новый экземпляр контекста Dapper.
    /// </summary>
    /// <param name="config">Конфигурация приложения с строкой подключения.</param>
    /// <param name="logger">Логгер.</param>
    public DapperContext(
        IConfiguration config,
        ILogger<DapperContext> logger)
    {
        _config = config;
        _logger = logger;
        _connectionString = _config.GetConnectionString("Postgres")
                            ?? throw new InvalidOperationException("Postgres connection string missing");

        _logger.LogInformation("Инициализирован DapperContext с подключением Postgres");
    }

    public IDbConnection CreateConnection()
    {
        _logger.LogDebug("Создание нового подключения к Postgres");
        return new NpgsqlConnection(_connectionString);
    }
}