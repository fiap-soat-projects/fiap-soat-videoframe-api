namespace Infrastructure.Clients.Interfaces;

public interface IS3BucketClient
{
    Task UploadAsync(
        string path,
        Stream content,
        CancellationToken cancellationToken);

    Task<Stream> DownloadAsync(string path, CancellationToken cancellationToken);

    Task<string> GetPreSignedDownloadUrlAsync(string path, CancellationToken cancellationToken);
}
