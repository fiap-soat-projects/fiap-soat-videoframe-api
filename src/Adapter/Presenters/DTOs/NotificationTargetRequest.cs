using Domain.Entities.Enums;

namespace Adapter.Presenters.DTOs;

public record NotificationTargetRequest(NotificationChannel Channel, string Target)
{
}
