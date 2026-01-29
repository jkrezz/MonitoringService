using Dapper;
using WebApi.Data;
using WebApi.Models;
using WebApi.Repositories.Interfaces;

namespace WebApi.Repositories;

/// <summary>
/// Репозиторий для работы с сущностями устройств в базе данных.
/// </summary>
/// <remarks>
/// Использует Dapper для выполнения SQL-запросов.
/// </remarks>
public class DeviceRepository : IDeviceRepository
{
    private readonly DapperContext _context;
    private readonly ILogger<DeviceRepository> _logger;

    /// <summary>
    /// Инициализирует новый экземпляр репозитория устройств.
    /// </summary>
    /// <param name="context">Контекст Dapper для создания подключений к БД.</param>
    /// <param name="logger">Логгер.</param>
    public DeviceRepository(DapperContext context, ILogger<DeviceRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Возвращает все устройства из базы данных.
    /// </summary>
    /// <returns>Коллекция устройств.</returns>
    public async Task<IEnumerable<Device>> GetAllAsync()
    {
        const string query = """
        SELECT id, name
        FROM devices
        ORDER BY name;
        """;

        using var connection = _context.CreateConnection();
        _logger.LogDebug("Выполнение запроса получения всех устройств");

        var devices = await connection.QueryAsync<Device>(query);
        var list = devices.ToList();

        _logger.LogInformation("Получено {Count} устройств из БД", list.Count);

        return list;
    }

    /// <summary>
    /// Создаёт новое или обновляет существующее устройство.
    /// </summary>
    /// <param name="device">Устройство.</param>
    public async Task UpsertAsync(Device device)
    {
        const string command = """
        INSERT INTO devices (id, name)
        VALUES (@Id, @Name)
        ON CONFLICT (id) DO UPDATE SET name = EXCLUDED.name;
        """;

        using var connection = _context.CreateConnection();
        _logger.LogDebug("Сохранение устройства {DeviceId}", device.Id);
        await connection.ExecuteAsync(command, device);
    }
}