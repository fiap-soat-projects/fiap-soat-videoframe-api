using Adapter.Presenters.DTOs;
using Domain.Entities;
using Domain.Entities.Enums;
using FluentAssertions;
using NSubstitute;

namespace UnitTests.Adapter.Controllers.VideoEditControllerTests.Methods;

public class StartAsyncTests : VideoEditControllerDependenciesMock
{
    [Fact]
    public async Task When_Valid_Id_And_UserRequest_Then_Calls_All_UseCase_Methods()
    {
        // Arrange
        var id = "edit-123";
        var userId = "user-123";
        var videoId = "video-123";
        var userRequest = new UserRequest(userId, "John Doe", "john@example.com");
        var cancellationToken = new CancellationToken();

        var videoEdit = new VideoEdit(
            id,
            DateTime.UtcNow,
            userId,
            "recipient@example.com",
            EditType.Frame,
            EditStatus.Created,
            videoId,
            null,
            new List<NotificationTarget>());

        var video = new Video(
            videoId,
            DateTime.UtcNow,
            userId,
            "/videos/test.mp4",
            "test.mp4",
            "video/mp4",
            1024000);

        _videoEditUseCaseMock.GetByIdAsync(id, userId, cancellationToken).Returns(videoEdit);
        _videoUseCaseMock.GetByIdAsync(videoId, userId, cancellationToken).Returns(video);
        _videoEditUseCaseMock.ProcessAsync(Arg.Any<Video>(), Arg.Any<VideoEdit>(), Arg.Any<User>(), cancellationToken).Returns(Task.CompletedTask);
        _videoEditUseCaseMock.UpdateStatusAsync(id, EditStatus.Processing, userId, cancellationToken).Returns(Task.CompletedTask);

        // Act
        await _sut.StartAsync(id, userRequest, cancellationToken);

        // Assert
        await _videoEditUseCaseMock.Received(1).GetByIdAsync(id, userId, cancellationToken);
        await _videoUseCaseMock.Received(1).GetByIdAsync(videoId, userId, cancellationToken);
        await _videoEditUseCaseMock.Received(1).ProcessAsync(Arg.Any<Video>(), Arg.Any<VideoEdit>(), Arg.Any<User>(), cancellationToken);
        await _videoEditUseCaseMock.Received(1).UpdateStatusAsync(id, EditStatus.Processing, userId, cancellationToken);
    }

    [Fact]
    public async Task When_VideoEditUseCase_GetByIdAsync_Throws_Exception_Then_Propagates_Exception()
    {
        // Arrange
        var id = "edit-123";
        var userId = "user-123";
        var userRequest = new UserRequest(userId, "John Doe", "john@example.com");
        var expectedException = new KeyNotFoundException("VideoEdit not found");

        _videoEditUseCaseMock.GetByIdAsync(id, userId, Arg.Any<CancellationToken>())
            .Returns(Task.FromException<VideoEdit>(expectedException));

        var cancellationToken = CancellationToken.None;

        // Act
        var act = async () => await _sut.StartAsync(id, userRequest, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task When_VideoUseCase_GetByIdAsync_Throws_Exception_Then_Propagates_Exception()
    {
        // Arrange
        var id = "edit-123";
        var userId = "user-123";
        var videoId = "video-123";
        var userRequest = new UserRequest(userId, "John Doe", "john@example.com");
        var expectedException = new KeyNotFoundException("Video not found");

        var videoEdit = new VideoEdit(
            id,
            DateTime.UtcNow,
            userId,
            "recipient@example.com",
            EditType.Frame,
            EditStatus.Created,
            videoId,
            null,
            new List<NotificationTarget>());

        _videoEditUseCaseMock.GetByIdAsync(id, userId, Arg.Any<CancellationToken>()).Returns(videoEdit);
        _videoUseCaseMock.GetByIdAsync(videoId, userId, Arg.Any<CancellationToken>())
            .Returns(Task.FromException<Video>(expectedException));

        var cancellationToken = CancellationToken.None;

        // Act
        var act = async () => await _sut.StartAsync(id, userRequest, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task When_ProcessAsync_Throws_Exception_Then_Propagates_Exception()
    {
        // Arrange
        var id = "edit-123";
        var userId = "user-123";
        var videoId = "video-123";
        var userRequest = new UserRequest(userId, "John Doe", "john@example.com");
        var expectedException = new InvalidOperationException("Process failed");
        var cancellationToken = new CancellationToken();

        var videoEdit = new VideoEdit(
            id,
            DateTime.UtcNow,
            userId,
            "recipient@example.com",
            EditType.Frame,
            EditStatus.Created,
            videoId,
            null,
            new List<NotificationTarget>());

        var video = new Video(
            videoId,
            DateTime.UtcNow,
            userId,
            "/videos/test.mp4",
            "test.mp4",
            "video/mp4",
            1024000);

        _videoEditUseCaseMock.GetByIdAsync(id, userId, cancellationToken).Returns(videoEdit);
        _videoUseCaseMock.GetByIdAsync(videoId, userId, cancellationToken).Returns(video);
        _videoEditUseCaseMock.ProcessAsync(Arg.Any<Video>(), Arg.Any<VideoEdit>(), Arg.Any<User>(), cancellationToken)
            .Returns(Task.FromException(expectedException));

        // Act
        var act = async () => await _sut.StartAsync(id, userRequest, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task When_UpdateStatusAsync_Throws_Exception_Then_Propagates_Exception()
    {
        // Arrange
        var id = "edit-123";
        var userId = "user-123";
        var videoId = "video-123";
        var userRequest = new UserRequest(userId, "John Doe", "john@example.com");
        var expectedException = new InvalidOperationException("Update status failed");
        var cancellationToken = new CancellationToken();

        var videoEdit = new VideoEdit(
            id,
            DateTime.UtcNow,
            userId,
            "recipient@example.com",
            EditType.Frame,
            EditStatus.Created,
            videoId,
            null,
            new List<NotificationTarget>());

        var video = new Video(
            videoId,
            DateTime.UtcNow,
            userId,
            "/videos/test.mp4",
            "test.mp4",
            "video/mp4",
            1024000);

        _videoEditUseCaseMock.GetByIdAsync(id, userId, cancellationToken).Returns(videoEdit);
        _videoUseCaseMock.GetByIdAsync(videoId, userId, cancellationToken).Returns(video);
        _videoEditUseCaseMock.ProcessAsync(Arg.Any<Video>(), Arg.Any<VideoEdit>(), Arg.Any<User>(), cancellationToken).Returns(Task.CompletedTask);
        _videoEditUseCaseMock.UpdateStatusAsync(id, EditStatus.Processing, userId, cancellationToken)
            .Returns(Task.FromException(expectedException));

        // Act
        var act = async () => await _sut.StartAsync(id, userRequest, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
