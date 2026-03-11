using Adapter.Presenters.DTOs;
using Domain.Entities;

namespace Adapter.Presenters;

public class GetVideoEditByIdPresenter
{
    public GetVideoEditResponse ViewModel { get; init; }

    public GetVideoEditByIdPresenter(VideoEdit videoEdit)
    {
        ViewModel = VideoEditToResponse(videoEdit);
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
