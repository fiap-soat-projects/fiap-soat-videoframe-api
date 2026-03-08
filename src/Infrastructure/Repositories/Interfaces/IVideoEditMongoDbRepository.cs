using Infrastructure.Repositories.Entities;

namespace Infrastructure.Repositories.Interfaces;

public interface IVideoEditMongoDbRepository
{
    Task<VideoEditMongoDb> GetByIdAsync(string id, string userId, CancellationToken cancellationToken);
    Task<IEnumerable<VideoEditMongoDb>> GetAllAsync(
        string userId, 
        int skip,
        int limit, 
        CancellationToken cancellationToken);

    Task<string> InsertOneAsync(VideoEditMongoDb videoEditMongoDb, CancellationToken cancellationToken);
    Task UpdateStatusAsync(string id, string userId, string status, CancellationToken cancellationToken);
    Task DeleteAsync(string id, string userId, CancellationToken cancellationToken);
}
