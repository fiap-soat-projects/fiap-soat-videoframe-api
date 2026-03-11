using Domain.Entities;
using Domain.Entities.Enums;
using FluentAssertions;

namespace UnitTests.Domain.Entities.VideoEditTests.Methods;

public class NotificationTargetsPropertyTests : VideoEditDependenciesMock
{
    [Fact]
    public void When_NotificationTargets_Is_Get_Then_Returns_Stored_Value()
    {
        // Act
        var result = _sut.NotificationTargets;

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<IEnumerable<NotificationTarget>>();
    }

    [Fact]
    public void When_NotificationTargets_Is_Set_With_Valid_Collection_Then_Updates_Property()
    {
        // Arrange
        var newNotificationTargets = new List<NotificationTarget>();

        // Act
        _sut.NotificationTargets = newNotificationTargets;

        // Assert
        _sut.NotificationTargets.Should().Equal(newNotificationTargets);
    }

    [Fact]
    public void When_NotificationTargets_Is_Set_With_Multiple_Items_Then_Updates_Property()
    {
        // Arrange
        var notificationTargets = new List<NotificationTarget>
        {
            new NotificationTarget(NotificationChannel.Email, "email1@example.com"),
            new NotificationTarget(NotificationChannel.Email, "email2@example.com")
        };

        // Act
        _sut.NotificationTargets = notificationTargets;

        // Assert
        _sut.NotificationTargets.Should().Equal(notificationTargets);
        _sut.NotificationTargets.Should().HaveCount(2);
    }

    [Fact]
    public void When_NotificationTargets_Is_Changed_Multiple_Times_Then_Property_Reflects_Latest_Value()
    {
        // Arrange
        var targets1 = new List<NotificationTarget>();
        var targets2 = new List<NotificationTarget>
        {
            new NotificationTarget(NotificationChannel.Email, "test@example.com")
        };
        var targets3 = new List<NotificationTarget>();

        // Act
        _sut.NotificationTargets = targets1;
        _sut.NotificationTargets = targets2;
        _sut.NotificationTargets = targets3;

        // Assert
        _sut.NotificationTargets.Should().Equal(targets3);
        _sut.NotificationTargets.Should().HaveCount(0);
    }

    [Fact]
    public void When_VideoEdit_Is_Created_With_NotificationTargets_Then_Uses_Provided_Targets()
    {
        // Arrange
        var notificationTargets = new List<NotificationTarget>
        {
            new NotificationTarget(NotificationChannel.Email, "target@example.com")
        };

        // Act
        var videoEdit = new VideoEdit("user-123", "recipient@example.com", EditType.Frame, EditStatus.Created, "video-123", notificationTargets);

        // Assert
        videoEdit.NotificationTargets.Should().Equal(notificationTargets);
        videoEdit.NotificationTargets.Should().HaveCount(1);
    }
}
