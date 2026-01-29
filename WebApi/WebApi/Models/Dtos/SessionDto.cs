namespace WebApi.Models.Dtos;

public class SessionDto
{
    public Guid Id { get; set; }
    public Guid DeviceId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Version { get; set; } = string.Empty;
}
