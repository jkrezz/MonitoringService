using WebApi.Models;

namespace WebApi.Models.Dtos;

public sealed class BackupDto
{
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    public IReadOnlyList<Device> Devices { get; init; } = Array.Empty<Device>();
    public IReadOnlyList<Session> Sessions { get; init; } = Array.Empty<Session>();
}

