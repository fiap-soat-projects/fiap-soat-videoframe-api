using Adapter.Presenters.DTOs;
using Business.Entities.Page;
using Domain.Entities;

namespace Adapter.Presenters;

public class GetPaginatedVideosPresenter
{
    public Pagination<GetVideoResponse> ViewModel { get; init; }

    public GetPaginatedVideosPresenter(Pagination<Video> videos)
    {
        ViewModel = new Pagination<GetVideoResponse>
        {
            Page = videos.Page,
            Size = videos.Size,
            TotalPages = videos.TotalPages,
            TotalCount = videos.TotalCount,
            Items = videos.Items.Select(VideoToResponse)
        };
    }

    private static GetVideoResponse VideoToResponse(Video video)
    {
        var videoResponse = new GetVideoResponse(video.Id!, video.Name!, video.Path!);
        return videoResponse;
    }

}
