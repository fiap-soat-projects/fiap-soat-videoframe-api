using Adapter.Presenters.DTOs;
using Domain.Entities;
using Domain.Entities.Page;

namespace Adapter.Presenters;

public class GetPaginatedVideoEditsPresenter
{
    public Pagination<GetVideoEditResponse> ViewModel { get; init; }

    public GetPaginatedVideoEditsPresenter(Pagination<VideoEdit> videoEdits)
    {
        ViewModel = new Pagination<GetVideoEditResponse>
        {
            Page = videoEdits.Page,
            Size = videoEdits.Size,
            TotalPages = videoEdits.TotalPages,
            TotalCount = videoEdits.TotalCount,
            Items = videoEdits.Items.Select(VideoEditToResponse)
        };
    }

    public static GetVideoEditResponse VideoEditToResponse(VideoEdit edition)
    {
        var editionResponses = new GetVideoEditResponse(
            edition.Id!,
            edition.Recipient!,
            edition.Type.ToString(),
            edition.Status.ToString(),
            edition.VideoId!);

        return editionResponses;
    }
}
