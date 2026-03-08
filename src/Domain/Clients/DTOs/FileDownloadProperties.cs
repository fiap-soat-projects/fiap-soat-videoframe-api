using Domain.Entities.Enums;

namespace Domain.Clients.DTOs;

public record FileDownloadProperties(string UserId, string FileName, string FileExtension, FileType FileType)
{
}
