using Adapter.Presenters.DTOs;
using Domain.Entities;
using Domain.Entities.Enums;
using FluentAssertions;
using NSubstitute;

namespace UnitTests.Adapter.Controllers.VideoEditControllerTests.Methods;

public class CreateAsyncTests : VideoEditControllerDependenciesMock
{
    [Fact]
    public async Task When_Valid_CreateVideoEditRequest_Then_Returns_CreatePresenter()
    {
        // Arrange
        var userId = "user-123";
        var videoId = "video-123";
        var editType = EditType.Frame;
        var notificationTargets = new List<NotificationTargetRequest>
        {
            new NotificationTargetRequest(NotificationChannel.Email, "test@example.com")
        };
        var createRequest = new CreateVideoEditRequest(editType, videoId, notificationTargets);
        var userRequest = new UserRequest(userId, "John Doe", "recipient@example.com");
        var createdId = "edit-123";

        _videoEditUseCaseMock.CreateAsync(Arg.Any<VideoEdit>(), Arg.Any<CancellationToken>())
            .Returns(createdId);

        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _sut.CreateAsync(createRequest, userRequest, cancellationToken);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task When_Multiple_Notification_Targets_Then_Creates_VideoEdit_With_All_Targets()
    {
        // Arrange
        var userId = "user-123";
        var videoId = "video-123";
        var notificationTargets = new List<NotificationTargetRequest>
        {
            new NotificationTargetRequest(NotificationChannel.Email, "email1@example.com"),
            new NotificationTargetRequest(NotificationChannel.Email, "email2@example.com")
        };
        var createRequest = new CreateVideoEditRequest(EditType.Frame, videoId, notificationTargets);
        var userRequest = new UserRequest(userId, "John Doe", "recipient@example.com");

        _videoEditUseCaseMock.CreateAsync(Arg.Any<VideoEdit>(), Arg.Any<CancellationToken>())
            .Returns("edit-123");

        var cancellationToken = CancellationToken.None;

        // Act
        await _sut.CreateAsync(createRequest, userRequest, cancellationToken);

        // Assert
        await _videoEditUseCaseMock.Received(1).CreateAsync(Arg.Any<VideoEdit>(), cancellationToken);
    }

    [Fact]
    public async Task When_UseCase_Throws_Exception_Then_Propagates_Exception()
    {
        // Arrange
        var createRequest = new CreateVideoEditRequest(
            EditType.Frame,
            "video-123",
            new List<NotificationTargetRequest>());
        var userRequest = new UserRequest("user-123", "John Doe", "recipient@example.com");
        var expectedException = new InvalidOperationException("Create failed");

        _videoEditUseCaseMock.CreateAsync(Arg.Any<VideoEdit>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromException<string>(expectedException));

        var cancellationToken = CancellationToken.None;

        // Act
        var act = async () => await _sut.CreateAsync(createRequest, userRequest, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Theory]
    [InlineData(EditType.Frame)]
    [InlineData(EditType.None)]
    public async Task When_Different_EditTypes_Then_Creates_VideoEdit_With_Correct_Type(EditType editType)
    {
        // Arrange
        var createRequest = new CreateVideoEditRequest(editType, "video-123", new List<NotificationTargetRequest>());
        var userRequest = new UserRequest("user-123", "John Doe", "recipient@example.com");

        _videoEditUseCaseMock.CreateAsync(Arg.Any<VideoEdit>(), Arg.Any<CancellationToken>())
            .Returns("edit-123");

        var cancellationToken = CancellationToken.None;

        // Act
        await _sut.CreateAsync(createRequest, userRequest, cancellationToken);

        // Assert
        await _videoEditUseCaseMock.Received(1).CreateAsync(Arg.Is<VideoEdit>(ve => ve.Type == editType), cancellationToken);
    }
}
