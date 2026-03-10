using Infrastructure.Entities;
using Infrastructure.Entities.Page;
using Infrastructure.MongoDb.Contexts.Interfaces;
using Infrastructure.Repositories.Abstractions;
using Infrastructure.Repositories.Interfaces;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

internal class VideoEditMongoDbRepository : BaseMongoDbRepository<VideoEditMongoDb>, IVideoEditMongoDbRepository
{
    public VideoEditMongoDbRepository(IMongoContext mongoContext) : base(mongoContext)
    {
    }

    public Task DeleteAsync(string id, string userId, CancellationToken cancellationToken)
    {
        var builder = new FilterDefinitionBuilder<VideoEditMongoDb>();

        var filter = builder.Eq(x => x.Id, id)
            & builder.Eq(x => x.UserId, userId);

        return _mongoCollection.DeleteOneAsync(filter, new(), cancellationToken);
    }

    public Task UpdateStatusAsync(string id, string userId, string status, CancellationToken cancellationToken)
    {
        var builder = new FilterDefinitionBuilder<VideoEditMongoDb>();

        var filter = builder.Eq(x => x.Id, id)
            & builder.Eq(x => x.UserId, userId);

        var update = Builders<VideoEditMongoDb>.Update
            .Set(x => x.Status, status);

        return _mongoCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }

    public async Task<PagedResult<VideoEditMongoDb>> GetAllAsync(
        string userId,
        int page,
        int size,
        CancellationToken cancellationToken)
    {
        var builder = new FilterDefinitionBuilder<VideoEditMongoDb>();
        var filter = builder.Eq(x => x.UserId, userId);

        var pagedResult = await GetPagedAsync(page, size, filter, cancellationToken: cancellationToken);

        return pagedResult;
    }

    public async Task<VideoEditMongoDb> GetByIdAsync(string id, string userId, CancellationToken cancellationToken)
    {
        var builder = new FilterDefinitionBuilder<VideoEditMongoDb>();

        var filter = builder.Eq(x => x.Id, id)
            & builder.Eq(x => x.UserId, userId);

        var cursor = await _mongoCollection.FindAsync(filter, new(), cancellationToken);
        var videoEdit = await cursor.FirstOrDefaultAsync(cancellationToken);

        return videoEdit;

    }

    public async Task<string> InsertOneAsync(VideoEditMongoDb videoEditMongoDb, CancellationToken cancellationToken)
    {
        await _mongoCollection.InsertOneAsync(videoEditMongoDb, new(), cancellationToken);
        return videoEditMongoDb.Id;
    }
}
