using WebApi.Models;

namespace WebApi.Repositories.Interfaces;

/// <summary>
/// Интерфейс репозитория для работы с устройствами.
/// </summary>
public interface IDeviceRepository
{
    Task<IEnumerable<Device>> GetAllAsync();
    Task UpsertAsync(Device device);
}
