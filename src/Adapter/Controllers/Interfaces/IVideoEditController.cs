using Adapter.Presenters;
using Adapter.Presenters.DTOs;
using Domain.Entities.Enums;

namespace Adapter.Controllers.Interfaces;

public interface IVideoEditController
{
    Task<string> CreateAsync(
        CreateVideoEditRequest createEditionRequest,
        UserRequest userRequest, 
        CancellationToken cancellationToken);

    Task UpdateStatusAsync(string id, EditStatus status, UserRequest userRequest, CancellationToken cancellationToken);
    Task<DownloadPresenter> DownloadAsync(string id, UserRequest userRequest, CancellationToken cancellationToken);
    Task StartAsync(string id, UserRequest userRequest, CancellationToken cancellationToken);
    Task<VideoLinkPresenter> GetLinkAsync(string id, UserRequest userRequest, CancellationToken cancellationToken);
    Task<GetPaginatedVideoEditsPresenter> GetPaginatedAsync(UserRequest userRequest, PaginationRequest paginationRequest, CancellationToken cancellationToken);
}
