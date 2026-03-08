namespace Infrastructure.Clients.Interfaces;

internal interface IS3BucketClient
{
    Task UploadAsync(
        string path,
        Stream content,
        CancellationToken cancellationToken);

    Task<Stream> DownloadAsync(string path, CancellationToken cancellationToken);
}
