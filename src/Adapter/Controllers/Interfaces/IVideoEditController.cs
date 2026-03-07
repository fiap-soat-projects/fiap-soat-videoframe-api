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
    Task<GetAllVideoEditsPresenter> GetAllAsync(UserRequest userRequest, PaginationRequest paginationRequest, CancellationToken cancellationToken);
}
