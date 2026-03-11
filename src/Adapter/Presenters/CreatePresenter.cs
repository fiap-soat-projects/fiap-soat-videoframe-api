using Adapter.Presenters.DTOs;

namespace Adapter.Presenters;

public class CreatePresenter
{
    public CreateResponse ViewModel { get; init; }

    public CreatePresenter(string id)
    {
        ViewModel = new CreateResponse(id);
    }
}
