using Domain.Entities;
using Domain.Entities.Enums;

namespace Domain.Producers.DTOs;

public record EditProcessorMessage(string EditId, string UserId, string UserName, string UserRecipient, string VideoPath, EditType EditType)
{
}
