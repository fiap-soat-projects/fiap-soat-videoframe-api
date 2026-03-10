using Adapter.Gateways.Extensions;
using Business.Entities.Page;
using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Gateways.Repositories.Interfaces;
using Infrastructure.Entities;
using Infrastructure.Repositories.Interfaces;

namespace Adapter.Gateways.Repositories;

internal class VideoEditRepository : IVideoEditRepository
{
    private readonly IVideoEditMongoDbRepository _videoEditMongoDbRepository;

    public VideoEditRepository(IVideoEditMongoDbRepository videoEditMongoDbRepository)
    {
        _videoEditMongoDbRepository = videoEditMongoDbRepository;
    }

    public Task DeleteAsync(string id, string userId, CancellationToken cancellationToken)
    {
        return _videoEditMongoDbRepository.DeleteAsync(id, userId, cancellationToken);
    }

    public async Task<Pagination<VideoEdit>> GetPaginatedAsync(string userId, int skip, int limit, CancellationToken cancellationToken)
    {
        var pagedResult = await _videoEditMongoDbRepository.GetAllAsync(userId, skip, limit, cancellationToken);
        var videoEdits = pagedResult.ToDomain();

        return videoEdits;
    }

    public async Task<VideoEdit?> GetByIdAsync(string id, string userId, CancellationToken cancellationToken)
    {
        var entity = await _videoEditMongoDbRepository.GetByIdAsync(id, userId, cancellationToken);
        var videoEdit = entity?.ToDomain();

        return videoEdit;
    }

    public Task<string> InsertOneAsync(VideoEdit videoEdit, CancellationToken cancellationToken)
    {
        var mongoDbEntity = new VideoEditMongoDb(videoEdit);

        return _videoEditMongoDbRepository.InsertOneAsync(mongoDbEntity, cancellationToken);
    }

    public Task UpdateStatusAsync(string id, string userId, EditStatus status, CancellationToken cancellationToken)
    {
        return _videoEditMongoDbRepository.UpdateStatusAsync(id, userId, status.ToString(), cancellationToken);
    }
}
