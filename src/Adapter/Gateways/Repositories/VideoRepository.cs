using Adapter.Gateways.Extensions;
using Domain.Entities;
using Domain.Repositories.Interfaces;
using Infrastructure.Repositories.Entities;
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

    public async Task<IEnumerable<Video>> GetAllAsync(string userId, int skip, int limit, CancellationToken cancellationToken)
    {
        var entities = await _videoMongoDbRepository.GetAllAsync(userId, skip, limit, cancellationToken);
        var videos = entities.Select(x => x.ToDomain());

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
