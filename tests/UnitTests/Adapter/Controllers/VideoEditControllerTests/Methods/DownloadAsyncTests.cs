using Adapter.Presenters.DTOs;
using Domain.Entities;
using Domain.Entities.Enums;
using FluentAssertions;
using NSubstitute;

namespace UnitTests.Adapter.Controllers.VideoEditControllerTests.Methods;

public class DownloadAsyncTests : VideoEditControllerDependenciesMock
{
    [Fact]
    public async Task When_VideoEdit_Status_Is_Processed_Then_Returns_DownloadPresenter()
    {
        // Arrange
        var id = "edit-123";
        var userId = "user-123";
        var userRequest = new UserRequest(userId, "John Doe", "recipient@example.com");
        var editPath = "/edits/edit-123.zip";
        var fileStream = new MemoryStream(new byte[] { 1, 2, 3 });

        var videoEdit = new VideoEdit(
            id,
            DateTime.UtcNow,
            userId,
            "recipient@example.com",
            EditType.Frame,
            EditStatus.Processed,
            "video-123",
            editPath,
            new List<NotificationTarget>());

        _videoEditUseCaseMock.GetByIdAsync(id, userId, Arg.Any<CancellationToken>())
            .Returns(videoEdit);

        _videoEditUseCaseMock.DownloadAsync(editPath, Arg.Any<CancellationToken>())
            .Returns(fileStream);

        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _sut.DownloadAsync(id, userRequest, cancellationToken);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task When_VideoEdit_Status_Is_Not_Processed_Then_Throws_Exception()
    {
        // Arrange
        var id = "edit-123";
        var userId = "user-123";
        var userRequest = new UserRequest(userId, "John Doe", "recipient@example.com");

        var videoEdit = new VideoEdit(
            id,
            DateTime.UtcNow,
            userId,
            "recipient@example.com",
            EditType.Frame,
            EditStatus.Processing,
            "video-123",
            "/path",
            new List<NotificationTarget>());

        _videoEditUseCaseMock.GetByIdAsync(id, userId, Arg.Any<CancellationToken>())
            .Returns(videoEdit);

        var cancellationToken = CancellationToken.None;

        // Act
        var act = async () => await _sut.DownloadAsync(id, userRequest, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }

    [Theory]
    [InlineData(EditStatus.Created)]
    [InlineData(EditStatus.Processing)]
    [InlineData(EditStatus.Error)]
    public async Task When_VideoEdit_Status_Is_Not_Processed_Then_Throws_For_Any_Non_Processed_Status(EditStatus status)
    {
        // Arrange
        var id = "edit-123";
        var userId = "user-123";
        var userRequest = new UserRequest(userId, "John Doe", "recipient@example.com");

        var videoEdit = new VideoEdit(
            id,
            DateTime.UtcNow,
            userId,
            "recipient@example.com",
            EditType.Frame,
            status,
            "video-123",
            "/path",
            new List<NotificationTarget>());

        _videoEditUseCaseMock.GetByIdAsync(id, userId, Arg.Any<CancellationToken>())
            .Returns(videoEdit);

        var cancellationToken = CancellationToken.None;

        // Act
        var act = async () => await _sut.DownloadAsync(id, userRequest, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task When_GetByIdAsync_Throws_Exception_Then_Propagates_Exception()
    {
        // Arrange
        var id = "edit-123";
        var userId = "user-123";
        var userRequest = new UserRequest(userId, "John Doe", "recipient@example.com");
        var expectedException = new KeyNotFoundException("VideoEdit not found");

        _videoEditUseCaseMock.GetByIdAsync(id, userId, Arg.Any<CancellationToken>())
            .Returns(Task.FromException<VideoEdit>(expectedException));

        var cancellationToken = CancellationToken.None;

        // Act
        var act = async () => await _sut.DownloadAsync(id, userRequest, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task When_DownloadAsync_UseCase_Throws_Exception_Then_Propagates_Exception()
    {
        // Arrange
        var id = "edit-123";
        var userId = "user-123";
        var userRequest = new UserRequest(userId, "John Doe", "recipient@example.com");
        var editPath = "/edits/edit-123.zip";
        var expectedException = new IOException("Download failed");

        var videoEdit = new VideoEdit(
            id,
            DateTime.UtcNow,
            userId,
            "recipient@example.com",
            EditType.Frame,
            EditStatus.Processed,
            "video-123",
            editPath,
            new List<NotificationTarget>());

        _videoEditUseCaseMock.GetByIdAsync(id, userId, Arg.Any<CancellationToken>())
            .Returns(videoEdit);

        _videoEditUseCaseMock.DownloadAsync(editPath, Arg.Any<CancellationToken>())
            .Returns(Task.FromException<Stream>(expectedException));

        var cancellationToken = CancellationToken.None;

        // Act
        var act = async () => await _sut.DownloadAsync(id, userRequest, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<IOException>();
    }
}
