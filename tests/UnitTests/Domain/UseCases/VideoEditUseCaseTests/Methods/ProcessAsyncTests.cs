using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Gateways.Producers.DTOs;
using Domain.ValueObjects;
using FluentAssertions;
using NSubstitute;

namespace UnitTests.Domain.UseCases.VideoEditUseCaseTests.Methods;

public class ProcessAsyncTests : VideoEditUseCaseDependenciesMock
{
    [Fact]
    public async Task When_Status_Is_Already_Started_Then_Throw_Exception()
    {
        // Arrange
        var video = new Video("userId", "path", "name", "ct", 1);
        var videoEdit = new VideoEdit("id1", System.DateTime.UtcNow, "userId", "recipient", EditType.Frame, EditStatus.Processed, "videoId", "editPath", new System.Collections.Generic.List<NotificationTarget>());
        var user = new User("u1", "User Name", new Email("user@example.com"));

        // Act
        var act = async () => await _sut.ProcessAsync(video, videoEdit, user, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<System.Exception>().WithMessage("This edit is already started");
    }

    [Fact]
    public async Task Have_Notification_Target_Appended_When_None_Then_Produce_Message()
    {
        // Arrange
        var video = new Video("userId", "path", "name", "ct", 1);
        var videoEdit = new VideoEdit("id2", System.DateTime.UtcNow, "userId", "recipient", EditType.Frame, EditStatus.Created, "videoId", "editPath", new System.Collections.Generic.List<NotificationTarget>());
        var user = new User("u2", "User Name", new Email("user2@example.com"));

        EditProcessorMessage? capturedMessage = null;

        _editProcessorProducer
            .When(x => x.ProduceAsync(Arg.Any<EditProcessorMessage>(), Arg.Any<CancellationToken>()))
            .Do(ci => capturedMessage = ci.ArgAt<EditProcessorMessage>(0));
        _editProcessorProducer.ProduceAsync(Arg.Any<EditProcessorMessage>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        // Act
        await _sut.ProcessAsync(video, videoEdit, user, CancellationToken.None);

        // Assert
        await _editProcessorProducer.Received(1).ProduceAsync(Arg.Any<EditProcessorMessage>(), Arg.Any<CancellationToken>());
        capturedMessage.Should().NotBeNull();
        capturedMessage!.NotificationTarget.Any(nt => nt.Target == user.Email.ToString()).Should().BeTrue();
    }
}
