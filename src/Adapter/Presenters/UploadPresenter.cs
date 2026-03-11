using Adapter.Presenters.DTOs;

namespace Adapter.Presenters;

public class UploadPresenter
{
    public CreateResponse ViewModel { get; init; }

    public UploadPresenter(string id)
    {
        ViewModel = new CreateResponse(id);
    }
}
