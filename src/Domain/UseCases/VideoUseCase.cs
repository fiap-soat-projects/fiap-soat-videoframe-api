using Business.Entities.Page;
using Domain.Entities;
using Domain.Entities.Exceptions;
using Domain.Gateways.Clients.DTOs;
using Domain.Gateways.Clients.Interfaces;
using Domain.Gateways.Repositories.Interfaces;
using Domain.UseCases.Interfaces;

namespace Domain.UseCases;

internal class VideoUseCase : IVideoUseCase
{
    private readonly IVideoRepository _videoRepository;
    private readonly IBucketClient _bucketClient;

    public VideoUseCase(IVideoRepository videoRepository, IBucketClient bucketClient)
    {
        _videoRepository = videoRepository;
        _bucketClient = bucketClient;
    }

    public async Task<string> CreateAsync(Video video, CancellationToken cancellationToken)
    {
        var id = await _videoRepository.InsertOneAsync(video, cancellationToken);
        return id;
    }

    public Task<Stream> DownloadAsync(string path, CancellationToken cancellationToken)
    {
        return _bucketClient.DownloadFileAsync(path, cancellationToken);
    }

    public async Task<Video> GetByIdAsync(string id, string userId, CancellationToken cancellationToken)
    {
        var video = await _videoRepository.GetByIdAsync(id, userId, cancellationToken);

        EntityNotFoundException<Video>.ThrowIfNull(video, id);

        return video!;
    }

    public async Task<Video?> GetByNameAsync(string name, string userId, CancellationToken cancellationToken)
    {
        var video = await _videoRepository.GetByNameAsync(name, userId, cancellationToken);

        return video;
    }

    public async Task<string> GetLinkAsync(string id, string userId, CancellationToken cancellationToken)
    {
        var video = await GetByIdAsync(id, userId, cancellationToken);

        var link = await _bucketClient.GetPreSignedDownloadUrlAsync(video.Path!, cancellationToken);

        return link;
    }

    public Task<Pagination<Video>> GetPaginatedAsync(string userId, int page, int size, CancellationToken cancellationToken)
    {
        var videos = _videoRepository.GetPaginatedAsync(userId, page, size, cancellationToken);

        return videos;
    }

    public Task<string> UploadAsync(FileUpload fileUpload, CancellationToken cancellationToken)
    {
        var path = _bucketClient.UploadFileAsync(fileUpload, cancellationToken);

        return path;
         
    }
}
