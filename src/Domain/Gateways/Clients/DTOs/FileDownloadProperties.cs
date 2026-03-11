using Domain.Entities.Enums;

namespace Domain.Gateways.Clients.DTOs;

public record FileDownloadProperties(string UserId, string FileName, string FileExtension, FileType FileType)
{
}
