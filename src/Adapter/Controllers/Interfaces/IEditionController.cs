using Adapter.Presenters.DTOs;

namespace Adapter.Controllers.Interfaces;

public interface IEditionController
{
    Task<string> CreateAsync(
        CreateEditionRequest createEditionRequest,
        UserRequest userRequest, 
        CancellationToken cancellationToken);

    Task UpdateStatusAsync(string id, string status, UserRequest userRequest, CancellationToken cancellationToken);
}
