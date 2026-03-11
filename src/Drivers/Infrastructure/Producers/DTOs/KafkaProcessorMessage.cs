using Domain.Entities;

namespace Infrastructure.Producers.DTOs;

public record KafkaProcessorMessage(string EditId, string UserId, string UserName, string UserRecipient, string VideoPath, string EditType, IEnumerable<NotificationTarget> NotificationTargets)
{
}
