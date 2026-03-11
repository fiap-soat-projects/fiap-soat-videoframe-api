using Domain.Entities;

namespace Infrastructure.Producers.DTOs;

public record KafkaProcessorMessage(string UserId, string UserName, string UserRecipient, string VideoPath, string EditType, IEnumerable<NotificationTarget> NotificationTargets)
{
}
