using System.Text.Json.Serialization;

namespace WebApi.Models.Dtos;

public class SessionCreateDto
{
    [JsonPropertyName("_id")]
    public Guid DeviceId { get; set; }

    public string Name { get; set; } = string.Empty;

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public string Version { get; set; } = string.Empty;
}
