using Infrastructure.Repositories.Entities;

namespace Infrastructure.Repositories.Interfaces;

internal interface IVideoMongoDbRepository
{
    Task<VideoMongoDb> GetByIdAsync(string id, string userId, CancellationToken cancellationToken);
    Task<IEnumerable<VideoMongoDb>> GetAllAsync(
        string userId,
        int skip,
        int limit,
        CancellationToken cancellationToken);

    Task<string> InsertOneAsync(VideoMongoDb videoEditMongoDb, CancellationToken cancellationToken);
    Task DeleteAsync(string id, string userId, CancellationToken cancellationToken);
}
