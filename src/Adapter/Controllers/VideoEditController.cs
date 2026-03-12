using Adapter.Controllers.Interfaces;
using Adapter.Presenters;
using Adapter.Presenters.DTOs;
using Domain.Entities;
using Domain.Entities.Enums;
using Domain.UseCases.Interfaces;

namespace Adapter.Controllers;

internal class VideoEditController : IVideoEditController
{
    private readonly IVideoEditUseCase _videoEditUseCase;
    private readonly IVideoUseCase _videoUseCase;

    public VideoEditController(IVideoEditUseCase videoEditUseCase, IVideoUseCase videoUseCase)
    {
        _videoEditUseCase = videoEditUseCase;
        _videoUseCase = videoUseCase;
    }

    public async Task<CreatePresenter> CreateAsync(CreateVideoEditRequest createvideoEditRequest, UserRequest userRequest, CancellationToken cancellationToken)
    {
        var notificationTargets = createvideoEditRequest
            .NotificationTargets
            .Select(x => new NotificationTarget(x.Channel, x.Target));

        var videoEdit = new VideoEdit(
            userRequest.Id, 
            userRequest.Recipient, 
            createvideoEditRequest.Type,
            EditStatus.Created,
            createvideoEditRequest.VideoId,
            notificationTargets);        

        var id = await _videoEditUseCase.CreateAsync(videoEdit, cancellationToken);
        return new CreatePresenter(id);
    }

    public async Task<DownloadPresenter> DownloadAsync(string id, UserRequest userRequest, CancellationToken cancellationToken)
    {
        var videoEdit = await _videoEditUseCase.GetByIdAsync(id, userRequest.Id, cancellationToken);

        if (videoEdit.Status != EditStatus.Processed) 
        {
            throw new Exception("This edit is not processed");
        }

        var fileStream = await _videoEditUseCase.DownloadAsync(videoEdit.EditPath!, cancellationToken);

        return new DownloadPresenter($"{videoEdit.Type}.zip", "application/octet-stream", fileStream);
    }

    public async Task<VideoLinkPresenter> GetLinkAsync(string id, UserRequest userRequest, CancellationToken cancellationToken)
    {
        var link = await _videoEditUseCase.GetLinkAsync(id, userRequest.Id, cancellationToken);

        return new VideoLinkPresenter(link);
    }

    public async Task<GetPaginatedVideoEditsPresenter> GetPaginatedAsync(UserRequest userRequest, PaginationRequest paginationRequest, CancellationToken cancellationToken)
    {
        var videoEdits = await _videoEditUseCase.GetPaginatedAsync(userRequest.Id, paginationRequest.Page, paginationRequest.Size, cancellationToken);

        return new GetPaginatedVideoEditsPresenter(videoEdits);
    }

    public async Task StartAsync(string id, UserRequest userRequest, CancellationToken cancellationToken)
    {
        var videoEdit = await _videoEditUseCase.GetByIdAsync(id, userRequest.Id, cancellationToken);
        var video = await _videoUseCase.GetByIdAsync(videoEdit.VideoId!, userRequest.Id, cancellationToken);
        var user = new User(userRequest.Id, userRequest.Name, userRequest.Recipient);

        await _videoEditUseCase.ProcessAsync(video, videoEdit, user, cancellationToken);

        await _videoEditUseCase.UpdateStatusAsync(id, EditStatus.Processing, user.Id!, cancellationToken);
    }

    public async Task UpdateStatusAsync(string id, EditStatus status, UserRequest userRequest, CancellationToken cancellationToken)
    {
        await _videoEditUseCase.UpdateStatusAsync(id, status, userRequest.Id, cancellationToken);
    }
}
