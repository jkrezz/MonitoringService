namespace WebApi.Models;

/// <summary>
/// Модель устройства.
/// </summary>
public class Device
{
    /// <summary>
    /// Уникальный идентификатор устройства.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public string Name { get; set; } = string.Empty;
}
