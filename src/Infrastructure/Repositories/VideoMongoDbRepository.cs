using Infrastructure.Entities;
using Infrastructure.Entities.Page;
using Infrastructure.MongoDb.Contexts.Interfaces;
using Infrastructure.Repositories.Abstractions;
using Infrastructure.Repositories.Interfaces;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

internal class VideoMongoDbRepository : BaseMongoDbRepository<VideoMongoDb>, IVideoMongoDbRepository
{
    public VideoMongoDbRepository(IMongoContext mongoContext) : base(mongoContext)
    {
    }

    public async Task DeleteAsync(string id, string userId, CancellationToken cancellationToken)
    {
        var builder = new FilterDefinitionBuilder<VideoMongoDb>();

        var filter = builder.Eq(x => x.Id, id)
            & builder.Eq(x => x.UserId, userId);

        await _mongoCollection.DeleteOneAsync(filter, new(), cancellationToken);
    }

    public async Task<PagedResult<VideoMongoDb>> GetAllAsync(string userId, int page, int size, CancellationToken cancellationToken)
    {
        var builder = new FilterDefinitionBuilder<VideoMongoDb>();
        var filter = builder.Eq(x => x.UserId, userId);
        var pagedResult = await GetPagedAsync(page, size, filter, cancellationToken: cancellationToken);

        return pagedResult;
    }

    public async Task<VideoMongoDb> GetByIdAsync(string id, string userId, CancellationToken cancellationToken)
    {
        var builder = new FilterDefinitionBuilder<VideoMongoDb>();

        var filter = builder.Eq(x => x.Id, id)
            & builder.Eq(x => x.UserId, userId);

        var cursor = await _mongoCollection.FindAsync(filter, new(), cancellationToken);
        var video = await cursor.FirstOrDefaultAsync(cancellationToken);

        return video;
    }

    public async Task<VideoMongoDb> GetByNameAsync(string name, string userId, CancellationToken cancellationToken)
    {
        var builder = new FilterDefinitionBuilder<VideoMongoDb>();

        var filter = builder.Eq(x => x.Name, name)
            & builder.Eq(x => x.UserId, userId);

        var cursor = await _mongoCollection.FindAsync(filter, new(), cancellationToken);
        var video = await cursor.FirstOrDefaultAsync(cancellationToken);

        return video;
    }

    public async Task<string> InsertOneAsync(VideoMongoDb videoMongoDb, CancellationToken cancellationToken)
    {
        await _mongoCollection.InsertOneAsync(videoMongoDb, new(), cancellationToken);
        return videoMongoDb.Id;
    }
}
