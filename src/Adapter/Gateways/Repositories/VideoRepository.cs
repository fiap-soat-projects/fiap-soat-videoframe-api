using Adapter.Gateways.Extensions;
using Business.Entities.Page;
using Domain.Entities;
using Domain.Gateways.Repositories.Interfaces;
using Infrastructure.Entities;
using Infrastructure.Repositories.Interfaces;

namespace Adapter.Gateways.Repositories;

internal class VideoRepository : IVideoRepository
{
    private readonly IVideoMongoDbRepository _videoMongoDbRepository;

    public VideoRepository(IVideoMongoDbRepository videoMongoDbRepository)
    {
        _videoMongoDbRepository = videoMongoDbRepository;
    }

    public Task DeleteAsync(string id, string userId, CancellationToken cancellationToken)
    {
        return _videoMongoDbRepository.DeleteAsync(id, userId, cancellationToken);
    }

    public async Task<Pagination<Video>> GetAllAsync(string userId, int page, int size, CancellationToken cancellationToken)
    {
        var pagedResult = await _videoMongoDbRepository.GetAllAsync(userId, page, size, cancellationToken);
        var videos = pagedResult.ToDomain();

        return videos;

    }

    public async Task<Video> GetByIdAsync(string id, string userId, CancellationToken cancellationToken)
    {
        var entity = await _videoMongoDbRepository.GetByIdAsync(id, userId, cancellationToken);
        var video = entity.ToDomain();
        return video;
    }

    public Task<string> InsertOneAsync(Video video, CancellationToken cancellationToken)
    {
        var videoMongoDb = new VideoMongoDb(video);

        return _videoMongoDbRepository.InsertOneAsync(videoMongoDb, cancellationToken);
    }
}
