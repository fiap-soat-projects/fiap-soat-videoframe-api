using Domain.Entities;
using Domain.Entities.Enums;
using Infrastructure.Entities;

namespace Adapter.Gateways.Extensions;

public static class NotificationTargetExtensions
{
    extension(NotificationTargetMongoDb notificationTargetMongoDb)
    {
        public NotificationTarget ToDomain()
        {
            var isNotificationChannelValid = Enum.TryParse<NotificationChannel>(notificationTargetMongoDb.Channel, out var channel);

            if (isNotificationChannelValid is false)
            {
                throw new Exception("Error while trying to parse videoEdit mongoDb entity");
            }

            var notificationTarget = new NotificationTarget(channel, notificationTargetMongoDb.Target);
            return notificationTarget;
        }
    }
}
