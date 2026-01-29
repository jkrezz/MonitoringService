using WebApi.Models;

namespace WebApi.Repositories.Interfaces;

/// <summary>
/// Интерфейс репозитория для работы с сессиями.
/// </summary>
public interface ISessionRepository
{
    Task CreateAsync(Session session);
    Task<IEnumerable<Session>> GetAllAsync();
    Task<IEnumerable<Session>> GetByDeviceAsync(Guid deviceId);
}
