using Domain.Entities;
using Domain.Entities.Enums;

namespace UnitTests.Domain.Entities.VideoEditTests;

public abstract class VideoEditDependenciesMock
{
    protected readonly VideoEdit _sut;

    protected VideoEditDependenciesMock(
        string? userId = "user-123",
        string? recipient = "recipient@example.com",
        EditType editType = EditType.Frame,
        EditStatus editStatus = EditStatus.Created,
        string? videoId = "video-123",
        IEnumerable<NotificationTarget>? notificationTargets = null)
    {
        notificationTargets ??= new List<NotificationTarget>();

        _sut = new VideoEdit(
            userId ?? "user-123",
            recipient ?? "recipient@example.com",
            editType,
            editStatus,
            videoId ?? "video-123",
            notificationTargets);
    }
}
