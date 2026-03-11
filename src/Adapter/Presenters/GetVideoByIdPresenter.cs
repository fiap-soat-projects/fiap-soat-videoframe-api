using Adapter.Presenters.DTOs;
using Domain.Entities;

namespace Adapter.Presenters;

public class GetVideoByIdPresenter
{
    public GetVideoResponse ViewModel { get; init; }

    public GetVideoByIdPresenter(Video video)
    {
        ViewModel = VideoToResponse(video);
    }

    private static GetVideoResponse VideoToResponse(Video video)
    {
        var videoResponse = new GetVideoResponse(video.Id!, video.Name!, video.Path!, video.ContentType!, video.ContentLength);
        return videoResponse;
    }
}
