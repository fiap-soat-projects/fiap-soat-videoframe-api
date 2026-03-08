using Infrastructure.Repositories.Abstractions;
using Infrastructure.Repositories.Entities;
using Infrastructure.Repositories.Interfaces;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

internal class VideoMongoDbRepository : BaseMongoDbRepository<VideoMongoDb>, IVideoMongoDbRepository
{
    internal VideoMongoDbRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
    {
    }

    public async Task DeleteAsync(string id, string userId, CancellationToken cancellationToken)
    {
        var builder = new FilterDefinitionBuilder<VideoMongoDb>();

        var filter = builder.Eq(x => x.Id, id)
            & builder.Eq(x => x.UserId, userId);

        await _mongoCollection.DeleteOneAsync(filter, new(), cancellationToken);
    }

    public async Task<IEnumerable<VideoMongoDb>> GetAllAsync(string userId, int skip, int limit, CancellationToken cancellationToken)
    {
        var builder = new FilterDefinitionBuilder<VideoMongoDb>();
        var options = new FindOptions<VideoMongoDb>()
        {
            Skip = skip,
            Limit = limit,
        };

        var filter = builder.Eq(x => x.UserId, userId);

        var cursor = await _mongoCollection.FindAsync(filter, options, cancellationToken);
        var videos = await cursor.ToListAsync(cancellationToken);

        return videos ?? [];
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

    public async Task<string> InsertOneAsync(VideoMongoDb videoMongoDb, CancellationToken cancellationToken)
    {
        await _mongoCollection.InsertOneAsync(videoMongoDb, new(), cancellationToken);
        return videoMongoDb.Id;
    }
}
