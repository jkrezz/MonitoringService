namespace WebApi.Models;

/// <summary>
/// Модель сессии работы устройства.
/// </summary>
public class Session
{
    /// <summary>
    /// Уникальный идентификатор сессии.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Идентификатор устройства, к которому относится сессия.
    /// </summary>
    public Guid DeviceId { get; set; }

    /// <summary>
    /// Время начала сессии (UTC).
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Время окончания сессии (UTC).
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// Версия программного обеспечения.
    /// </summary>
    public string Version { get; set; } = string.Empty;
}
