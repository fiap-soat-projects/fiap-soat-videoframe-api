using Domain.Entities;
using Domain.Entities.Enums;

namespace Domain.Gateways.Producers.DTOs;

public record EditProcessorMessage(string EditId, string UserId, string UserName, string UserRecipient, string VideoPath, EditType EditType, IEnumerable<NotificationTarget> NotificationTarget)
{
}
