using Domain.Entities.Enums;

namespace Domain.Gateways.Clients.DTOs;

public record FileUpload(string UserId, string Name, long Lentgh, string Type, Stream FileStream)
{
}
