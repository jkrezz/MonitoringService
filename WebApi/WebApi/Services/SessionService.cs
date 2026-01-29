using WebApi.Models;
using WebApi.Models.Dtos;
using WebApi.Repositories.Interfaces;
using WebApi.Services.Interfaces;

namespace WebApi.Services;

/// <summary>
/// Сервис для работы с сессиями.
/// </summary>
public class SessionService : ISessionService
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly ISessionRepository _sessionRepository;
    private readonly ILogger<SessionService> _logger;

    /// <summary>
    /// Инициализирует новый экземпляр сервиса сессий.
    /// </summary>
    /// <param name="deviceRepository">Репозиторий устройств.</param>
    /// <param name="sessionRepository">Репозиторий сессий.</param>
    /// <param name="logger">Логгер.</param>
    public SessionService(
        IDeviceRepository deviceRepository,
        ISessionRepository sessionRepository,
        ILogger<SessionService> logger)
    {
        _deviceRepository = deviceRepository;
        _sessionRepository = sessionRepository;
        _logger = logger;
    }

    /// <summary>
    /// Создаёт новую сессию и при необходимости сохраняет информацию об устройстве.
    /// </summary>
    /// <param name="dto">Данные для создания сессии.</param>
    public async Task CreateSessionAsync(SessionCreateDto dto)
    {
        _logger.LogInformation(
            "Начало создания сессии для устройства {DeviceId} с интервалом {StartTime} - {EndTime}",
            dto.DeviceId,
            dto.StartTime,
            dto.EndTime);

        var device = new Device
        {
            Id = dto.DeviceId,
            Name = dto.Name
        };

        var session = new Session
        {
            Id = Guid.NewGuid(),
            DeviceId = dto.DeviceId,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            Version = dto.Version
        };

        await _deviceRepository.UpsertAsync(device);
        _logger.LogDebug("Устройство {DeviceId} сохранено/обновлено", dto.DeviceId);

        await _sessionRepository.CreateAsync(session);
        _logger.LogInformation(
            "Сессия {SessionId} для устройства {DeviceId} успешно сохранена",
            session.Id,
            dto.DeviceId);
    }

    /// <summary>
    /// Возвращает список сессий для указанного устройства.
    /// </summary>
    /// <param name="deviceId">Идентификатор устройства.</param>
    /// <returns>Коллекция сессий устройства.</returns>
    public async Task<IEnumerable<SessionDto>> GetSessionsByDeviceAsync(Guid deviceId)
    {
        _logger.LogInformation("Запрос сессий для устройства {DeviceId} в сервисе", deviceId);

        var sessions = await _sessionRepository.GetByDeviceAsync(deviceId);
        var result = sessions.Select(s => new SessionDto
        {
            Id = s.Id,
            DeviceId = s.DeviceId,
            StartTime = s.StartTime,
            EndTime = s.EndTime,
            Version = s.Version
        }).ToList();

        _logger.LogInformation(
            "Сервис вернул {Count} сессий для устройства {DeviceId}",
            result.Count,
            deviceId);

        return result;
    }
}
