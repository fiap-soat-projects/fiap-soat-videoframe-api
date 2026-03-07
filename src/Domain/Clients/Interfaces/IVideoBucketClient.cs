using Domain.Clients.DTOs;

namespace Domain.Clients.Interfaces;

public interface IVideoBucketClient
{
    Task<string> UploadFileAsync(FileUpload file, CancellationToken cancellationToken);
    Task<Stream> DownloadFileAsync(string filePath, CancellationToken cancellationToken);
}
