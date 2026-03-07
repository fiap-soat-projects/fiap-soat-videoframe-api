namespace Domain.Clients.DTOs;

public record FileUpload(string FilePath, Stream FileStream)
{
}
