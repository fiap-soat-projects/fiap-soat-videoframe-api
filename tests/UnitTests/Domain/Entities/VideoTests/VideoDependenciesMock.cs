using Domain.Entities;

namespace UnitTests.Domain.Entities.VideoTests;

public abstract class VideoDependenciesMock
{
    protected readonly Video _sut;

    protected VideoDependenciesMock(
        string? userId = "user-123",
        string? path = "/videos/test.mp4",
        string? name = "test-video.mp4",
        string? contentType = "video/mp4",
        long contentLength = 1024000)
    {
        _sut = new Video(
            userId ?? "user-123",
            path ?? "/videos/test.mp4",
            name ?? "test-video.mp4",
            contentType ?? "video/mp4",
            contentLength);
    }
}
