using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Dtos;
using WebApi.Models;
using WebApi.Repositories.Interfaces;
using WebApi.Exception;

namespace WebApi.Controllers;

/// <summary>
/// Контроллер для управления устройствами.
/// </summary>
/// <remarks>
/// Предоставляет методы для получения списка устройств и создания/обновления информации об устройстве.
/// </remarks>
[ApiController]
[Route("api/[controller]")]
public class DevicesController : ControllerBase
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly ILogger<DevicesController> _logger;

    /// <summary>
    /// Создаёт новый экземпляр контроллера устройств.
    /// </summary>
    /// <param name="deviceRepository">Репозиторий для работы с устройствами.</param>
    /// <param name="logger">Логгер.</param>
    public DevicesController(
        IDeviceRepository deviceRepository,
        ILogger<DevicesController> logger)
    {
        _deviceRepository = deviceRepository;
        _logger = logger;
    }

    /// <summary>
    /// Возвращает список всех устройств.
    /// </summary>
    /// <returns>Коллекция устройств.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DeviceDto>>> GetDevices()
    {
        _logger.LogInformation("Получен запрос на получение списка устройств");

        var devices = await _deviceRepository.GetAllAsync();
        var response = devices.Select(d => new DeviceDto { Id = d.Id, Name = d.Name }).ToList();

        _logger.LogInformation("Возврат {Count} устройств", response.Count);

        return Ok(response);
    }

    /// <summary>
    /// Создаёт или обновляет устройство.
    /// </summary>
    /// <param name="dto">Данные устройства для создания или обновления.</param>
    /// <returns>Результат создания.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateDevice([FromBody] DeviceCreateDto dto)
    {
        if (dto.Id == Guid.Empty)
        {
            _logger.LogWarning("Попытка создать устройство без Id");
            throw new BadRequestException("Id устройства должен быть задан");
        }

        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            _logger.LogWarning("Попытка создать устройство с пустым именем для Id {DeviceId}", dto.Id);
            throw new BadRequestException("Имя пользователя не может быть пустым");
        }

        var device = new Device
        {
            Id = dto.Id,
            Name = dto.Name
        };

        _logger.LogInformation("Создание устройства {DeviceId}", dto.Id);
        await _deviceRepository.UpsertAsync(device);
        _logger.LogInformation("Устройство {DeviceId} успешно сохранено", dto.Id);

        return CreatedAtAction(nameof(GetDevices), null);
    }
}