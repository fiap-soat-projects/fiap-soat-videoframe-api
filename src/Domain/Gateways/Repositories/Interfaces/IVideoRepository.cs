using Business.Entities.Page;
using Domain.Entities;

namespace Domain.Gateways.Repositories.Interfaces;

public interface IVideoRepository
{
    Task<Video> GetByIdAsync(string id, string userId, CancellationToken cancellationToken);
    Task<Pagination<Video>> GetAllAsync(
        string userId,
        int skip,
        int limit,
        CancellationToken cancellationToken);

    Task<string> InsertOneAsync(Video video, CancellationToken cancellationToken);
    Task DeleteAsync(string id, string userId, CancellationToken cancellationToken);
}
