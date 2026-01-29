using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Dtos;
using WebApi.Repositories.Interfaces;

namespace WebApi.Controllers;

/// <summary>
/// Контроллер для формирования и скачивания бэкапа данных.
/// </summary>
/// <remarks>
/// Позволяет выгружать устройства и сессии в JSON-файл.
/// </remarks>
[ApiController]
[Route("api/[controller]")]
public class BackupController : ControllerBase
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    private readonly IDeviceRepository _deviceRepository;
    private readonly ISessionRepository _sessionRepository;
    private readonly ILogger<BackupController> _logger;

    /// <summary>
    /// Создаёт новый экземпляр контроллера бэкапа.
    /// </summary>
    /// <param name="deviceRepository">Репозиторий устройств.</param>
    /// <param name="sessionRepository">Репозиторий сессий.</param>
    /// <param name="logger">Логгер.</param>
    public BackupController(
        IDeviceRepository deviceRepository,
        ISessionRepository sessionRepository,
        ILogger<BackupController> logger)
    {
        _deviceRepository = deviceRepository;
        _sessionRepository = sessionRepository;
        _logger = logger;
    }

    /// <summary>
    /// Формирует и возвращает бэкап устройств и сессий.
    /// </summary>
    /// <returns>Файл JSON с данными устройств и сессий.</returns>
    [HttpGet]
    public async Task<IActionResult> DownloadBackup()
    {
        _logger.LogInformation("Получен запрос на бэкап данных в JSON");

        var devices = (await _deviceRepository.GetAllAsync()).ToList();
        var sessions = (await _sessionRepository.GetAllAsync()).ToList();

        var backup = new BackupDto
        {
            CreatedAt = DateTimeOffset.UtcNow,
            Devices = devices,
            Sessions = sessions
        };

        var json = JsonSerializer.Serialize(backup, JsonOptions);
        var bytes = Encoding.UTF8.GetBytes(json);

        var fileName = $"backup-{DateTime.UtcNow:yyyyMMdd-HHmmss}.json";
        _logger.LogInformation(
            "Сформирован бэкап {FileName}: devices={DevicesCount}, sessions={SessionsCount}",
            fileName,
            devices.Count,
            sessions.Count);

        return File(bytes, "application/json; charset=utf-8", fileName);
    }
}

