using Adapter.Presenters.DTOs;
using Domain.Entities;

namespace Adapter.Presenters;

public class GetAllVideosPresenter
{
    public IEnumerable<GetVideoResponse> Videos { get; init; }

    public GetAllVideosPresenter(IEnumerable<Video> videos)
    {
        Videos = videos.Select(VideoToResponse);
    }

    private static GetVideoResponse VideoToResponse(Video video)
    {
        var videoResponse = new GetVideoResponse(video.Id!, video.Name, video.Path);
        return videoResponse;
    }

}
