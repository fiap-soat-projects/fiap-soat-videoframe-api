using Domain.Clients.DTOs;
using Domain.Clients.Interfaces;
using Domain.Entities.Enums;
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
        var fileNameWithExtension = string.Join('.', file.Name, GetFileExtensionString(file.Type));

        var path = string.Format(PATH_TEMPLATE, file.UserId, file.Type.ToString(), fileNameWithExtension);

        using (file.FileStream)
        {
            await _s3BucketClient.UploadAsync(path, file.FileStream, cancellationToken);
        }

        return path;
    }

    private static string GetFileExtensionString(FileType fileType)
    {
        return fileType switch 
        { 
            FileType.Video => "mp4",
            FileType.VideoEdit => "zip",
            _ => "bin"        
        };
    }
}
