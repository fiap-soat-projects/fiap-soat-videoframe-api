using Domain.Entities;
using Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Entities;

public class NotificationTargetMongoDb
{
    public NotificationTargetMongoDb(NotificationTarget notificationTarget)
    {
        Channel = notificationTarget.Channel.ToString();
        Target = notificationTarget.Target;
    }

    public string? Channel { get; init; }
    public string? Target { get; init; }
}
