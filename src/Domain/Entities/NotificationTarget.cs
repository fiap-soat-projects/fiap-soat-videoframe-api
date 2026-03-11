using Domain.Entities.Enums;
using Domain.Entities.Exceptions;
using Domain.Entities.Interfaces;
using Domain.ValueObjects;

namespace Domain.Entities;

public class NotificationTarget : IDomainEntity
{
    public NotificationTarget(NotificationChannel channel, string? target)
    {
        Channel = channel;

        Target = channel switch
        {
            NotificationChannel.Email => new Email(target),
            NotificationChannel.Webhook => new Webhook(target),
            _ => throw new InvalidOperationException("This target format is invalid")

        };
    }

    public NotificationChannel Channel { get; set; }
    public string? Target 
    {
        get;
        set
        {
            InvalidEntityPropertyException<NotificationTarget>.ThrowIfNullOrWhiteSpace(value, nameof(Target));
            field = value;
        }
    }
}
