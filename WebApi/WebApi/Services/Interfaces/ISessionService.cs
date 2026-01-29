using WebApi.Models.Dtos;

namespace WebApi.Services.Interfaces;

/// <summary>
/// Интерфейс сервиса для работы с сессиями устройств.
/// </summary>
public interface ISessionService
{
    Task CreateSessionAsync(SessionCreateDto dto);
    Task<IEnumerable<SessionDto>> GetSessionsByDeviceAsync(Guid deviceId);
}
