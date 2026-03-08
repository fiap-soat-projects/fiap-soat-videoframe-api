using Domain.Gateways.Clients.DTOs;

namespace Domain.Gateways.Clients.Interfaces;

public interface IBucketClient
{
    Task<string> UploadFileAsync(FileUpload file, CancellationToken cancellationToken);
    Task<Stream> DownloadFileAsync(string filePath, CancellationToken cancellationToken);
    Task<string> GetPreSignedDownloadUrlAsync(string filePath, CancellationToken cancellationToken);
}
