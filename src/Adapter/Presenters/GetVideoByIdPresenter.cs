using Adapter.Presenters.DTOs;
using Domain.Entities;

namespace Adapter.Presenters;

public class GetVideoByIdPresenter
{
    public GetVideoResponse Video { get; init; }

    public GetVideoByIdPresenter(Video video)
    {
        Video = VideoToResponse(video);
    }

    private static GetVideoResponse VideoToResponse(Video video)
    {
        var videoResponse = new GetVideoResponse(video.Id!, video.Name, video.Path);
        return videoResponse;
    }
}
