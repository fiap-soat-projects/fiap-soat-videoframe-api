using Adapter.Presenters.DTOs;
using Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace UnitTests.Adapter.Controllers.VideoControllerTests.Methods;

public class DownloadAsyncTests : VideoControllerDependenciesMock
{
    [Fact]
    public async Task When_Valid_Id_And_UserRequest_Then_Returns_DownloadPresenter()
    {
        // Arrange
        var id = "video-123";
        var userId = "user-123";
        var userRequest = new UserRequest(userId, "John Doe", "recipient@example.com");
        var videoPath = "/videos/test.mp4";
        var videoName = "test-video.mp4";
        var videoContentType = "video/mp4";
        var videoStream = new MemoryStream(new byte[] { 1, 2, 3 });

        var video = new Video(
            id,
            DateTime.UtcNow,
            userId,
            videoPath,
            videoName,
            videoContentType,
            1024000);

        _videoUseCaseMock.GetByIdAsync(id, userId, Arg.Any<CancellationToken>())
            .Returns(video);

        _videoUseCaseMock.DownloadAsync(videoPath, Arg.Any<CancellationToken>())
            .Returns(videoStream);

        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _sut.DownloadAsync(id, userRequest, cancellationToken);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task When_GetByIdAsync_Throws_Exception_Then_Propagates_Exception()
    {
        // Arrange
        var id = "video-123";
        var userId = "user-123";
        var userRequest = new UserRequest(userId, "John Doe", "recipient@example.com");
        var expectedException = new InvalidOperationException("Video not found");

        _videoUseCaseMock.GetByIdAsync(id, userId, Arg.Any<CancellationToken>())
            .Returns(Task.FromException<Video>(expectedException));

        var cancellationToken = CancellationToken.None;

        // Act
        var act = async () => await _sut.DownloadAsync(id, userRequest, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task When_DownloadAsync_Throws_Exception_Then_Propagates_Exception()
    {
        // Arrange
        var id = "video-123";
        var userId = "user-123";
        var userRequest = new UserRequest(userId, "John Doe", "recipient@example.com");
        var videoPath = "/videos/test.mp4";
        var expectedException = new IOException("Download failed");

        var video = new Video(
            id,
            DateTime.UtcNow,
            userId,
            videoPath,
            "test-video.mp4",
            "video/mp4",
            1024000);

        _videoUseCaseMock.GetByIdAsync(id, userId, Arg.Any<CancellationToken>())
            .Returns(video);

        _videoUseCaseMock.DownloadAsync(videoPath, Arg.Any<CancellationToken>())
            .Returns(Task.FromException<Stream>(expectedException));

        var cancellationToken = CancellationToken.None;

        // Act
        var act = async () => await _sut.DownloadAsync(id, userRequest, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<IOException>();
    }

    [Fact]
    public async Task When_Called_Then_Calls_UseCase_With_Correct_Parameters()
    {
        // Arrange
        var id = "video-123";
        var userId = "user-123";
        var userRequest = new UserRequest(userId, "John Doe", "recipient@example.com");
        var videoPath = "/videos/test.mp4";
        var cancellationToken = new CancellationToken();

        var video = new Video(
            id,
            DateTime.UtcNow,
            userId,
            videoPath,
            "test.mp4",
            "video/mp4",
            1024000);

        _videoUseCaseMock.GetByIdAsync(id, userId, cancellationToken).Returns(video);
        _videoUseCaseMock.DownloadAsync(videoPath, cancellationToken).Returns(new MemoryStream());

        // Act
        await _sut.DownloadAsync(id, userRequest, cancellationToken);

        // Assert
        await _videoUseCaseMock.Received(1).GetByIdAsync(id, userId, cancellationToken);
        await _videoUseCaseMock.Received(1).DownloadAsync(videoPath, cancellationToken);
    }
}
