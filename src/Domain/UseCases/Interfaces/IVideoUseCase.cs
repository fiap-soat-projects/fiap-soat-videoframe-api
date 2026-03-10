using Business.Entities.Page;
using Domain.Entities;
using Domain.Gateways.Clients.DTOs;

namespace Domain.UseCases.Interfaces;

public interface IVideoUseCase
{
    Task<string> CreateAsync(Video video, CancellationToken cancellationToken);
    Task<string> UploadAsync(FileUpload fileUpload, CancellationToken cancellationToken);
    Task<Stream> DownloadAsync(string path, CancellationToken cancellationToken);
    Task<string> GetLinkAsync(string id, string userId, CancellationToken cancellationToken);
    Task<Pagination<Video>> GetPaginatedAsync(string userId, int page, int size, CancellationToken cancellationToken);
    Task<Video> GetByIdAsync(string id, string userId, CancellationToken cancellationToken);
    Task<Video?> GetByNameAsync(string name, string userId, CancellationToken cancellationToken);
}
