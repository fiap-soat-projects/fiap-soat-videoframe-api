using Adapter.Controllers.Interfaces;
using Adapter.Presenters;
using Adapter.Presenters.DTOs;
using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Gateways.Clients.DTOs;
using Domain.UseCases.Interfaces;

namespace Adapter.Controllers;

internal class VideoController : IVideoController
{
    private readonly IVideoUseCase _videoUseCase;

    public VideoController(IVideoUseCase videoUseCase)
    {
        _videoUseCase = videoUseCase;
    }

    public async Task<DownloadPresenter> DownloadAsync(string id, UserRequest userRequest, CancellationToken cancellationToken)
    {
        var video = await _videoUseCase.GetByIdAsync(id, userRequest.Id, cancellationToken);

        var videoStream =  await _videoUseCase.DownloadAsync(video.Path!, cancellationToken);

        return new DownloadPresenter(video.Name!, video.ContentType!, videoStream);
    }

    public async Task<GetPaginatedVideosPresenter> GetPaginatedAsync(UserRequest userRequest, PaginationRequest paginationRequest, CancellationToken cancellationToken)
    {
        var paginatedVideos = await _videoUseCase.GetPaginatedAsync(userRequest.Id, paginationRequest.Page, paginationRequest.Size, cancellationToken);

        return new GetPaginatedVideosPresenter(paginatedVideos);
    }

    public async Task<GetVideoByIdPresenter> GetByIdAsync(string id, UserRequest userRequest, CancellationToken cancellationToken)
    {
        var video = await _videoUseCase.GetByIdAsync(id, userRequest.Id, cancellationToken);
        return new GetVideoByIdPresenter(video);
    }

    public async Task<VideoLinkPresenter> GetLinkAsync(string id, UserRequest userRequest, CancellationToken cancellationToken)
    {
        var link = await _videoUseCase.GetLinkAsync(id, userRequest.Id, cancellationToken);

        return new VideoLinkPresenter(link);
    }

    public async Task<UploadPresenter> UploadAsync(UploadVideoRequest uploadVideoRequest, UserRequest userRequest, CancellationToken cancellationToken)
    {
        var fileUpload = new FileUpload(userRequest.Id, uploadVideoRequest.FileName, FileType.Video, uploadVideoRequest.Content);

        var path = await _videoUseCase.UploadAsync(fileUpload, cancellationToken);

        var video = new Video(userRequest.Id, path, uploadVideoRequest.FileName, uploadVideoRequest.ContentType);

        var id = await _videoUseCase.CreateAsync(video, cancellationToken);

        return new UploadPresenter(id); 
    }
}
