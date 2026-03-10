using Domain.Entities.Enums;
using Domain.Gateways.Clients.DTOs;
using Domain.Gateways.Clients.Interfaces;
using Infrastructure.Clients.Interfaces;

namespace Adapter.Gateways.Clients;

internal class BucketClient : IBucketClient
{
    private readonly IS3BucketClient _s3BucketClient;
    private const string PATH_TEMPLATE = "users/{0}/{1}/{2}";

    public BucketClient(IS3BucketClient s3BucketClient)
    {
        _s3BucketClient = s3BucketClient;
    }

    public Task<Stream> DownloadFileAsync(string filePath, CancellationToken cancellationToken)
    {
        return _s3BucketClient.DownloadAsync(filePath, cancellationToken);
    }

    public async Task<string> UploadFileAsync(FileUpload file, CancellationToken cancellationToken)
    {
        var path = string.Format(PATH_TEMPLATE, file.UserId, file.Type.ToString(), file.Name);

        await _s3BucketClient.UploadAsync(path, file.FileStream, cancellationToken);    

        return path;
    }

    public Task<string> GetPreSignedDownloadUrlAsync(string filePath, CancellationToken cancellationToken)
    {
        return _s3BucketClient.GetPreSignedDownloadUrlAsync(filePath, cancellationToken);
    }
}
