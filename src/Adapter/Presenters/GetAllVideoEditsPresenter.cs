using Adapter.Presenters.DTOs;
using Domain.Entities;

namespace Adapter.Presenters;

public class GetAllVideoEditsPresenter
{
    public IEnumerable<GetVideoEditResponse> Edits { get; init; }

    public GetAllVideoEditsPresenter(IEnumerable<VideoEdit> editionContexts)
    {
        Edits = editionContexts.Select(VideEditToResponse);
    }

    public static GetVideoEditResponse VideEditToResponse(VideoEdit edition)
    {
        var editionResponses = new GetVideoEditResponse(
            edition.Id!,
            edition.Recipient,
            edition.Type.ToString(),
            edition.Status.ToString(),
            edition.VideoId);

        return editionResponses;
    }
}
