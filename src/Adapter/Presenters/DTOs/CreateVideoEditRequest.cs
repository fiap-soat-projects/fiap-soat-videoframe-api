using Domain.Entities.Enums;

namespace Adapter.Presenters.DTOs;

public record CreateVideoEditRequest(EditType Type, string VideoId, IEnumerable<NotificationTargetRequest> NotificationTargets)
{
}
