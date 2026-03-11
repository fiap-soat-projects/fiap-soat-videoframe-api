using Adapter.Presenters;
using Adapter.Presenters.DTOs;

namespace Adapter.Controllers.Interfaces;

public interface IVideoController
{
    Task<UploadPresenter> UploadAsync(UploadVideoRequest uploadVideoRequest, UserRequest userRequest, CancellationToken cancellationToken);
    Task<DownloadPresenter> DownloadAsync(string id, UserRequest userRequest, CancellationToken cancellationToken);
    Task<VideoLinkPresenter> GetLinkAsync(string id, UserRequest userRequest, CancellationToken cancellationToken);
    Task<GetPaginatedVideosPresenter> GetPaginatedAsync(UserRequest userRequest, PaginationRequest paginationRequest, CancellationToken cancellationToken);
    Task<GetVideoByIdPresenter> GetByIdAsync(string id, UserRequest userRequest, CancellationToken cancellationToken);
}
