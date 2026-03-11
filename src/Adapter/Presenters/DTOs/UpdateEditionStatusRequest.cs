using Domain.Entities.Enums;

namespace Adapter.Presenters.DTOs;

public record UpdateEditionStatusRequest(string userId, EditStatus Status)
{
}
