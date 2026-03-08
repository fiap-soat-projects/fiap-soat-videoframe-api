using Domain.Entities;
using Domain.Entities.Enums;

namespace Domain.Gateways.Repositories.Interfaces;

public interface IVideoEditRepository
{
    Task<VideoEdit> GetByIdAsync(string id, string userId, CancellationToken cancellationToken);
    Task<IEnumerable<VideoEdit>> GetAllAsync(
        string userId,
        int skip,
        int limit,
        CancellationToken cancellationToken);

    Task<string> InsertOneAsync(VideoEdit videoEdit, CancellationToken cancellationToken);
    Task UpdateStatusAsync(string id, string userId, EditStatus status, CancellationToken cancellationToken);
    Task DeleteAsync(string id, string userId, CancellationToken cancellationToken);
}
