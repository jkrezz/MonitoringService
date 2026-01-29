namespace WebApi.Models.Dtos;

public class DeviceCreateDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}