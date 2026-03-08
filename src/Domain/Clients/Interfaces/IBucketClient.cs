using Domain.Clients.DTOs;

namespace Domain.Clients.Interfaces;

public interface IBucketClient
{
    Task<string> UploadFileAsync(FileUpload file, CancellationToken cancellationToken);
    Task<Stream> DownloadFileAsync(string filePath, CancellationToken cancellationToken);
}
