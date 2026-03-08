using Infrastructure.Entities;
using Infrastructure.Entities.Page;

namespace Infrastructure.Repositories.Interfaces;

public interface IVideoMongoDbRepository
{
    Task<VideoMongoDb> GetByIdAsync(string id, string userId, CancellationToken cancellationToken);
    Task<PagedResult<VideoMongoDb>> GetAllAsync(
        string userId,
        int page,
        int size,
        CancellationToken cancellationToken);

    Task<string> InsertOneAsync(VideoMongoDb videoMongoDb, CancellationToken cancellationToken);
    Task DeleteAsync(string id, string userId, CancellationToken cancellationToken);
}
