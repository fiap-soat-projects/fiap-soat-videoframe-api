using Adapter.Controllers.Interfaces;
using Adapter.Presenters;
using Adapter.Presenters.DTOs;
using Domain.UseCases.Interfaces;

namespace Adapter.Controllers;

internal class VideoController : IVideoController
{
    private readonly IVideoUseCase _videoUseCase;

    public VideoController(IVideoUseCase videoUseCase)
    {
        _videoUseCase = videoUseCase;
    }

    public Task<DownloadPresenter> DownloadAsync(string id, UserRequest userRequest, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<GetPaginatedVideosPresenter> GetPaginatedAsync(UserRequest userRequest, PaginationRequest paginationRequest, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<GetVideoByIdPresenter> GetByIdAsync(string id, UserRequest userRequest, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<VideoLinkPresenter> GetLinkAsync(string id, UserRequest userRequest, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<UploadPresenter> UploadAsync(UploadVideoRequest uploadVideoRequest, UserRequest userRequest, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
