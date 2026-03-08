using Domain.Entities;

namespace Domain.Repositories.Interfaces;

public interface IVideoRepository
{
    Task<Video> GetByIdAsync(string id, string userId, CancellationToken cancellationToken);
    Task<IEnumerable<Video>> GetAllAsync(
        string userId,
        int skip,
        int limit,
        CancellationToken cancellationToken);

    Task<string> InsertOneAsync(Video video, CancellationToken cancellationToken);
    Task DeleteAsync(string id, string userId, CancellationToken cancellationToken);
}
