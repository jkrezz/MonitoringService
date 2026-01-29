using Microsoft.AspNetCore.Mvc;
using WebApi.Exception;
using WebApi.Models.Dtos;
using WebApi.Services.Interfaces;

namespace WebApi.Controllers;

/// <summary>
/// Контроллер для работы с сессиями устройств.
/// </summary>
/// <remarks>
/// Позволяет создавать сессии и получать список сессий для конкретного устройства.
/// </remarks>
[ApiController]
public class SessionsController : ControllerBase
{
    private readonly ISessionService _sessionService;
    private readonly ILogger<SessionsController> _logger;

    /// <summary>
    /// Создаёт новый экземпляр контроллера сессий.
    /// </summary>
    /// <param name="sessionService">Логина сессий.</param>
    /// <param name="logger">Логгер.</param>
    public SessionsController(
        ISessionService sessionService,
        ILogger<SessionsController> logger)
    {
        _sessionService = sessionService;
        _logger = logger;
    }

    /// <summary>
    /// Создаёт новую сессию для устройства.
    /// </summary>
    /// <param name="dto">Данные для создания сессии.</param>
    /// <returns>Результат создания.</returns>
    [HttpPost("api/sessions")]
    public async Task<IActionResult> CreateSession([FromBody] SessionCreateDto dto)
    {
        _logger.LogInformation(
            "Получен запрос на создание сессии для устройства {DeviceId} с интервалом {StartTime} - {EndTime}",
            dto.DeviceId,
            dto.StartTime,
            dto.EndTime);

        if (dto.EndTime < dto.StartTime)
        {
            _logger.LogWarning(
                "Некорректный интервал сессии для устройства {DeviceId}: endTime {EndTime} раньше startTime {StartTime}",
                dto.DeviceId,
                dto.EndTime,
                dto.StartTime);
            throw new BadRequestException("endTime не может быть раньше startTime");
        }

        await _sessionService.CreateSessionAsync(dto);

        _logger.LogInformation("Сессия для устройства {DeviceId} успешно создана", dto.DeviceId);

        return CreatedAtAction(nameof(GetSessionsByDevice), new { id = dto.DeviceId }, null);
    }

    /// <summary>
    /// Возвращает список сессий для указанного устройства.
    /// </summary>
    /// <param name="id">Идентификатор устройства.</param>
    /// <returns>Коллекция сессий устройства.</returns>
    [HttpGet("api/devices/{id:guid}/sessions")]
    public async Task<ActionResult<IEnumerable<SessionDto>>> GetSessionsByDevice(Guid id)
    {
        _logger.LogInformation("Получен запрос на получение сессий для устройства {DeviceId}", id);

        var response = await _sessionService.GetSessionsByDeviceAsync(id);

        if (!response.Any())
        {
            _logger.LogWarning("Сессии для устройства {DeviceId} не найдены", id);
            throw new NotFoundException($"Сессии для устройства {id} не найдены");
        }

        _logger.LogInformation("Найдено {Count} сессий для устройства {DeviceId}", response.Count(), id);

        return Ok(response);
    }
}