using Dapper;
using WebApi.Data;
using WebApi.Models;
using WebApi.Repositories.Interfaces;

namespace WebApi.Repositories;

/// <summary>
/// Репозиторий для работы с сущностями сессий в базе данных.
/// </summary>
/// <remarks>
/// Отвечает за создание и выборку сессий с использованием Dapper.
/// </remarks>
public class SessionRepository : ISessionRepository
{
    private readonly DapperContext _context;
    private readonly ILogger<SessionRepository> _logger;

    /// <summary>
    /// Инициализирует новый экземпляр репозитория сессий.
    /// </summary>
    /// <param name="context">Контекст Dapper для создания подключений к БД.</param>
    /// <param name="logger">Логгер.</param>
    public SessionRepository(DapperContext context, ILogger<SessionRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Создаёт новую запись сессии.
    /// </summary>
    /// <param name="session">Сессия.</param>
    public async Task CreateAsync(Session session)
    {
        const string command = """
        INSERT INTO sessions (id, device_id, start_time, end_time, version)
        VALUES (@Id, @DeviceId, @StartTime, @EndTime, @Version);
        """;

        using var connection = _context.CreateConnection();
        _logger.LogDebug("Создание сессии {SessionId} для устройства {DeviceId}", session.Id, session.DeviceId);
        await connection.ExecuteAsync(command, session);
    }

    /// <summary>
    /// Возвращает все сессии из базы данных.
    /// </summary>
    /// <returns>Коллекция сессий.</returns>
    public async Task<IEnumerable<Session>> GetAllAsync()
    {
        const string query = """
        SELECT id, device_id AS DeviceId, start_time AS StartTime, end_time AS EndTime, version
        FROM sessions
        ORDER BY start_time DESC;
        """;

        using var connection = _context.CreateConnection();
        _logger.LogDebug("Запрос всех сессий из БД");

        var result = await connection.QueryAsync<Session>(query);
        var list = result.ToList();

        _logger.LogInformation("Из БД получено {Count} сессий", list.Count);

        return list;
    }

    /// <summary>
    /// Возвращает все сессии для указанного устройства.
    /// </summary>
    /// <param name="deviceId">Идентификатор устройства.</param>
    /// <returns>Коллекция сессий по устройству.</returns>
    public async Task<IEnumerable<Session>> GetByDeviceAsync(Guid deviceId)
    {
        const string query = """
        SELECT id, device_id AS DeviceId, start_time AS StartTime, end_time AS EndTime, version
        FROM sessions
        WHERE device_id = @DeviceId
        ORDER BY start_time DESC;
        """;

        using var connection = _context.CreateConnection();
        _logger.LogDebug("Запрос сессий для устройства {DeviceId} из БД", deviceId);

        var result = await connection.QueryAsync<Session>(query, new { DeviceId = deviceId });
        var list = result.ToList();

        _logger.LogInformation(
            "Из БД получено {Count} сессий для устройства {DeviceId}",
            list.Count,
            deviceId);

        return list;
    }
}