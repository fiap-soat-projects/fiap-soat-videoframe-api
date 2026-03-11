using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Entities.Exceptions;
using FluentAssertions;

namespace UnitTests.Domain.Entities.VideoEditTests.Constructor;

public class VideoEditConstructorTests
{
    [Fact]
    public void When_Valid_Parameters_With_Id_Then_Construction_Succeeds()
    {
        // Arrange
        var id = "edit-123";
        var createdAt = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc);
        var userId = "user-123";
        var recipient = "recipient@example.com";
        var type = EditType.Frame;
        var status = EditStatus.Created;
        var videoId = "video-123";
        var editPath = "/edits/edit-123";
        var notificationTargets = new List<NotificationTarget>();

        // Act
        var videoEdit = new VideoEdit(id, createdAt, userId, recipient, type, status, videoId, editPath, notificationTargets);

        // Assert
        videoEdit.Id.Should().Be(id);
        videoEdit.CreatedAt.Should().Be(createdAt);
        videoEdit.UserId.Should().Be(userId);
        videoEdit.Recipient.Should().Be(recipient);
        videoEdit.Type.Should().Be(type);
        videoEdit.Status.Should().Be(status);
        videoEdit.VideoId.Should().Be(videoId);
        videoEdit.EditPath.Should().Be(editPath);
        videoEdit.NotificationTargets.Should().Equal(notificationTargets);
    }

    [Fact]
    public void When_Valid_Parameters_Without_Id_Then_Construction_Succeeds()
    {
        // Arrange
        var userId = "user-123";
        var recipient = "recipient@example.com";
        var type = EditType.Frame;
        var status = EditStatus.Created;
        var videoId = "video-123";
        var notificationTargets = new List<NotificationTarget>();

        // Act
        var videoEdit = new VideoEdit(userId, recipient, type, status, videoId, notificationTargets);

        // Assert
        videoEdit.UserId.Should().Be(userId);
        videoEdit.Recipient.Should().Be(recipient);
        videoEdit.Type.Should().Be(type);
        videoEdit.Status.Should().Be(status);
        videoEdit.VideoId.Should().Be(videoId);
        videoEdit.NotificationTargets.Should().Equal(notificationTargets);
        videoEdit.Id.Should().BeNull();
        videoEdit.EditPath.Should().BeNull();
    }

    [Theory]
    [InlineData(EditType.None, EditStatus.None)]
    [InlineData(EditType.Frame, EditStatus.Created)]
    [InlineData(EditType.Frame, EditStatus.Processing)]
    [InlineData(EditType.Frame, EditStatus.Processed)]
    public void When_Different_Valid_Edit_Types_And_Statuses_Then_Construction_Succeeds(EditType type, EditStatus status)
    {
        // Arrange
        var notificationTargets = new List<NotificationTarget>();

        // Act
        var videoEdit = new VideoEdit("user-123", "recipient@example.com", type, status, "video-123", notificationTargets);

        // Assert
        videoEdit.Type.Should().Be(type);
        videoEdit.Status.Should().Be(status);
    }

    [Fact]
    public void When_UserId_Is_Null_Then_Throw_InvalidEntityPropertyException()
    {
        // Arrange
        string? nullUserId = null;

        // Act
        var act = () => new VideoEdit(
            nullUserId,
            "recipient@example.com",
            EditType.Frame,
            EditStatus.Created,
            "video-123",
            new List<NotificationTarget>());

        // Assert
        act.Should()
            .Throw<InvalidEntityPropertyException<VideoEdit>>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public void When_UserId_Is_Whitespace_Then_Throw_InvalidEntityPropertyException(string whitespaceUserId)
    {
        // Act
        var act = () => new VideoEdit(
            whitespaceUserId,
            "recipient@example.com",
            EditType.Frame,
            EditStatus.Created,
            "video-123",
            new List<NotificationTarget>());

        // Assert
        act.Should()
            .Throw<InvalidEntityPropertyException<VideoEdit>>();
    }

    [Fact]
    public void When_Recipient_Is_Null_Then_Throw_InvalidEntityPropertyException()
    {
        // Arrange
        string? nullRecipient = null;

        // Act
        var act = () => new VideoEdit(
            "user-123",
            nullRecipient,
            EditType.Frame,
            EditStatus.Created,
            "video-123",
            new List<NotificationTarget>());

        // Assert
        act.Should()
            .Throw<InvalidEntityPropertyException<VideoEdit>>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public void When_Recipient_Is_Whitespace_Then_Throw_InvalidEntityPropertyException(string whitespaceRecipient)
    {
        // Act
        var act = () => new VideoEdit(
            "user-123",
            whitespaceRecipient,
            EditType.Frame,
            EditStatus.Created,
            "video-123",
            new List<NotificationTarget>());

        // Assert
        act.Should()
            .Throw<InvalidEntityPropertyException<VideoEdit>>();
    }

    [Fact]
    public void When_VideoId_Is_Null_Then_Throw_InvalidEntityPropertyException()
    {
        // Arrange
        string? nullVideoId = null;

        // Act
        var act = () => new VideoEdit(
            "user-123",
            "recipient@example.com",
            EditType.Frame,
            EditStatus.Created,
            nullVideoId,
            new List<NotificationTarget>());

        // Assert
        act.Should()
            .Throw<InvalidEntityPropertyException<VideoEdit>>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public void When_VideoId_Is_Whitespace_Then_Throw_InvalidEntityPropertyException(string whitespaceVideoId)
    {
        // Act
        var act = () => new VideoEdit(
            "user-123",
            "recipient@example.com",
            EditType.Frame,
            EditStatus.Created,
            whitespaceVideoId,
            new List<NotificationTarget>());

        // Assert
        act.Should()
            .Throw<InvalidEntityPropertyException<VideoEdit>>();
    }

    [Fact]
    public void When_Id_Is_Null_In_Full_Constructor_Then_Throw_InvalidEntityPropertyException()
    {
        // Arrange
        string? nullId = null;

        // Act
        var act = () => new VideoEdit(
            nullId,
            DateTime.UtcNow,
            "user-123",
            "recipient@example.com",
            EditType.Frame,
            EditStatus.Created,
            "video-123",
            "/path",
            new List<NotificationTarget>());

        // Assert
        act.Should()
            .Throw<InvalidEntityPropertyException<VideoEdit>>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public void When_Id_Is_Whitespace_In_Full_Constructor_Then_Throw_InvalidEntityPropertyException(string whitespaceId)
    {
        // Act
        var act = () => new VideoEdit(
            whitespaceId,
            DateTime.UtcNow,
            "user-123",
            "recipient@example.com",
            EditType.Frame,
            EditStatus.Created,
            "video-123",
            "/path",
            new List<NotificationTarget>());

        // Assert
        act.Should()
            .Throw<InvalidEntityPropertyException<VideoEdit>>();
    }
}
