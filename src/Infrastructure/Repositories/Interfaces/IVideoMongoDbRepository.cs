using Infrastructure.Repositories.Entities;

namespace Infrastructure.Repositories.Interfaces;

public interface IVideoMongoDbRepository
{
    Task<VideoMongoDb> GetByIdAsync(string id, string userId, CancellationToken cancellationToken);
    Task<IEnumerable<VideoMongoDb>> GetAllAsync(
        string userId,
        int skip,
        int limit,
        CancellationToken cancellationToken);

    Task<string> InsertOneAsync(VideoMongoDb videoMongoDb, CancellationToken cancellationToken);
    Task DeleteAsync(string id, string userId, CancellationToken cancellationToken);
}
