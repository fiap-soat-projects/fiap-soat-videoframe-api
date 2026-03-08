using Infrastructure.Repositories.Abstractions;
using Infrastructure.Repositories.Entities;
using Infrastructure.Repositories.Interfaces;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

internal class VideoEditMongoDbRepository : BaseMongoDbRepository<VideoEditMongoDb>, IVideoEditMongoDbRepository
{
    internal VideoEditMongoDbRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
    {
    }

    public async Task DeleteAsync(string id, string userId, CancellationToken cancellationToken)
    {
        var builder = new FilterDefinitionBuilder<VideoEditMongoDb>();

        var filter = builder.Eq(x => x.Id, id)
            & builder.Eq(x => x.UserId, userId);

        await _mongoCollection.DeleteOneAsync(filter, new(), cancellationToken);
    }

    public Task UpdateStatusAsync(string id, string userId, string status, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<VideoEditMongoDb>> GetAllAsync(
        string userId,
        int skip,
        int limit,
        CancellationToken cancellationToken)
    {
        var builder = new FilterDefinitionBuilder<VideoEditMongoDb>();
        var options = new FindOptions<VideoEditMongoDb>()
        {
            Skip = skip,
            Limit = limit,
        };

        var filter = builder.Eq(x => x.UserId, userId);

        var cursor = await _mongoCollection.FindAsync(filter, options, cancellationToken);
        var videoEdits = await cursor.ToListAsync(cancellationToken);

        return videoEdits ?? [];
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
