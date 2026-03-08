using Business.Entities.Page;
using Domain.Entities;
using Domain.Entities.Enums;

namespace Domain.UseCases.Interfaces;

public interface IVideoEditUseCase
{
    Task ProcessAsync(
        Video video,
        VideoEdit videoEdit,
        User user,
        CancellationToken cancellationToken);

    Task<string> CreateAsync(
        VideoEdit videoEdit,
        CancellationToken cancellationToken);

    Task UpdateStatusAsync(string id, EditStatus status, string userId, CancellationToken cancellationToken);
    Task<Stream> DownloadAsync(string path, CancellationToken cancellationToken);
    Task<string> GetLinkAsync(string id, string userId, CancellationToken cancellationToken);
    Task<VideoEdit> GetByIdAsync(string id, string userId, CancellationToken cancellationToken);
    Task<Pagination<VideoEdit>> GetPaginatedAsync(string userId, int page, int size, CancellationToken cancellationToken);
}
