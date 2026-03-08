using System;
using System.IO;
using System.Threading;
using Amazon.S3;
using Amazon.S3.Model;
using Infrastructure.Clients.Interfaces;
using Infrastructure.Providers;

namespace Infrastructure.Clients;

internal class S3BucketClient : IS3BucketClient
{
    private readonly string _bucketName;
    private readonly AmazonS3Client _client;

    public S3BucketClient(AmazonS3Client amazonS3Client, string bucketName)
    {
        _client = amazonS3Client;
        _bucketName = bucketName;
    }

    public async Task<Stream> DownloadAsync(string path, CancellationToken cancellationToken)
    {
        var getRequest = new GetObjectRequest
        {
            BucketName = _bucketName,
            Key = path
        };

        var response = await _client.GetObjectAsync(getRequest, cancellationToken).ConfigureAwait(false);

        return response.ResponseStream; 
    }

    public async Task UploadAsync(string path, Stream content, CancellationToken cancellationToken)
    {
        if (content == null) throw new ArgumentNullException(nameof(content));

        if (content.CanSeek)
        {
            content.Position = 0;
        }

        var putRequest = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = path,
            InputStream = content,
            ContentType = "video/mp4"
        };

        await _client.PutObjectAsync(putRequest, cancellationToken).ConfigureAwait(false);
    }


}
