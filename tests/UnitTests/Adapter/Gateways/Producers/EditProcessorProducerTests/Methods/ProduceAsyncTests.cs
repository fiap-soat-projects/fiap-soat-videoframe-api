using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Gateways.Producers.DTOs;
using FluentAssertions;
using Infrastructure.Producers.DTOs;
using NSubstitute;

namespace UnitTests.Adapter.Gateways.Producers.EditProcessorProducerTests.Methods;

public class ProduceAsyncTests : EditProcessorProducerDependenciesMock
{
    [Fact]
    public async Task When_Valid_EditProcessorMessage_Then_Calls_Kafka_Producer()
    {
        // Arrange
        var editId = "edit-123";
        var userName = "John Doe";
        var userRecipient = "john@example.com";
        var videoPath = "/videos/test.mp4";
        var editType = EditType.Frame;
        var notificationTargets = new List<NotificationTarget>();

        var message = new EditProcessorMessage(
            editId,
            "user-123",
            userName,
            userRecipient,
            videoPath,
            editType,
            notificationTargets);

        var cancellationToken = CancellationToken.None;

        _kafkaProcessorProducerMock.ProduceAsync(Arg.Any<KafkaProcessorMessage>(), cancellationToken)
            .Returns(Task.CompletedTask);

        // Act
        await _sut.ProduceAsync(message, cancellationToken);

        // Assert
        await _kafkaProcessorProducerMock.Received(1).ProduceAsync(Arg.Any<KafkaProcessorMessage>(), cancellationToken);
    }

    [Fact]
    public async Task When_EditProcessorMessage_Is_Produced_Then_Kafka_Message_Has_Correct_EditType_String()
    {
        // Arrange
        var editType = EditType.Frame;
        var message = new EditProcessorMessage(
            "edit-123",
            "user-123",
            "John Doe",
            "john@example.com",
            "/videos/test.mp4",
            editType,
            new List<NotificationTarget>());

        var cancellationToken = CancellationToken.None;

        _kafkaProcessorProducerMock.ProduceAsync(Arg.Any<KafkaProcessorMessage>(), cancellationToken)
            .Returns(Task.CompletedTask);

        // Act
        await _sut.ProduceAsync(message, cancellationToken);

        // Assert
        await _kafkaProcessorProducerMock.Received(1).ProduceAsync(
            Arg.Is<KafkaProcessorMessage>(km => km.EditType == editType.ToString()),
            cancellationToken);
    }

    [Fact]
    public async Task When_Multiple_NotificationTargets_Then_All_Are_Passed_To_Kafka()
    {
        // Arrange
        var notificationTargets = new List<NotificationTarget>
        {
            new NotificationTarget(NotificationChannel.Email, "email1@example.com"),
            new NotificationTarget(NotificationChannel.Email, "email2@example.com")
        };

        var message = new EditProcessorMessage(
            "edit-123",
            "user-123",
            "John Doe",
            "john@example.com",
            "/videos/test.mp4",
            EditType.Frame,
            notificationTargets);

        var cancellationToken = CancellationToken.None;

        _kafkaProcessorProducerMock.ProduceAsync(Arg.Any<KafkaProcessorMessage>(), cancellationToken)
            .Returns(Task.CompletedTask);

        // Act
        await _sut.ProduceAsync(message, cancellationToken);

        // Assert
        await _kafkaProcessorProducerMock.Received(1).ProduceAsync(
            Arg.Is<KafkaProcessorMessage>(km => km.NotificationTargets.Count() == 2),
            cancellationToken);
    }

    [Theory]
    [InlineData(EditType.Frame)]
    [InlineData(EditType.None)]
    public async Task When_Different_EditTypes_Then_Converts_To_String_Correctly(EditType editType)
    {
        // Arrange
        var message = new EditProcessorMessage(
            "edit-123",
            "user-123",
            "John Doe",
            "john@example.com",
            "/videos/test.mp4",
            editType,
            new List<NotificationTarget>());

        var cancellationToken = CancellationToken.None;

        _kafkaProcessorProducerMock.ProduceAsync(Arg.Any<KafkaProcessorMessage>(), cancellationToken)
            .Returns(Task.CompletedTask);

        // Act
        await _sut.ProduceAsync(message, cancellationToken);

        // Assert
        await _kafkaProcessorProducerMock.Received(1).ProduceAsync(
            Arg.Is<KafkaProcessorMessage>(km => km.EditType == editType.ToString()),
            cancellationToken);
    }

    [Fact]
    public async Task When_Message_Fields_Then_Maps_To_Kafka_Message_Correctly()
    {
        // Arrange
        var editId = "edit-123";
        var userId = "user-123";
        var userName = "John Doe";
        var userRecipient = "john@example.com";
        var videoPath = "/videos/test.mp4";
        var editType = EditType.Frame;
        var notificationTargets = new List<NotificationTarget>();

        var message = new EditProcessorMessage(editId, userId, userName, userRecipient, videoPath, editType, notificationTargets);
        var cancellationToken = CancellationToken.None;

        _kafkaProcessorProducerMock.ProduceAsync(Arg.Any<KafkaProcessorMessage>(), cancellationToken)
            .Returns(Task.CompletedTask);

        // Act
        await _sut.ProduceAsync(message, cancellationToken);

        // Assert
        await _kafkaProcessorProducerMock.Received(1).ProduceAsync(
            Arg.Is<KafkaProcessorMessage>(km =>
                km.EditId == editId &&
                km.UserId == userId &&
                km.UserName == userName &&
                km.UserRecipient == userRecipient &&
                km.VideoPath == videoPath),
            cancellationToken);
    }

    [Fact]
    public async Task When_Kafka_Producer_Throws_Exception_Then_Propagates_Exception()
    {
        // Arrange
        var message = new EditProcessorMessage(
            "edit-123",
            "user-123",
            "John Doe",
            "john@example.com",
            "/videos/test.mp4",
            EditType.Frame,
            new List<NotificationTarget>());

        var expectedException = new InvalidOperationException("Kafka produce failed");
        var cancellationToken = CancellationToken.None;

        _kafkaProcessorProducerMock.ProduceAsync(Arg.Any<KafkaProcessorMessage>(), cancellationToken)
            .Returns(Task.FromException(expectedException));

        // Act
        var act = async () => await _sut.ProduceAsync(message, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task When_Multiple_Messages_Produced_Then_All_Calls_Are_Made()
    {
        // Arrange
        var message1 = new EditProcessorMessage(
            "edit-1",
            "user-1",
            "John Doe",
            "john@example.com",
            "/videos/video1.mp4",
            EditType.Frame,
            new List<NotificationTarget>());

        var message2 = new EditProcessorMessage(
            "edit-2",
            "user-2",
            "Jane Doe",
            "jane@example.com",
            "/videos/video2.mp4",
            EditType.None,
            new List<NotificationTarget>());

        var cancellationToken = CancellationToken.None;

        _kafkaProcessorProducerMock.ProduceAsync(Arg.Any<KafkaProcessorMessage>(), cancellationToken)
            .Returns(Task.CompletedTask);

        // Act
        await _sut.ProduceAsync(message1, cancellationToken);
        await _sut.ProduceAsync(message2, cancellationToken);

        // Assert
        await _kafkaProcessorProducerMock.Received(2).ProduceAsync(Arg.Any<KafkaProcessorMessage>(), cancellationToken);
    }

    [Fact]
    public async Task When_Empty_NotificationTargets_Then_Passes_Empty_Collection()
    {
        // Arrange
        var message = new EditProcessorMessage(
            "edit-123",
            "user-123",
            "John Doe",
            "john@example.com",
            "/videos/test.mp4",
            EditType.Frame,
            new List<NotificationTarget>());

        var cancellationToken = CancellationToken.None;

        _kafkaProcessorProducerMock.ProduceAsync(Arg.Any<KafkaProcessorMessage>(), cancellationToken)
            .Returns(Task.CompletedTask);

        // Act
        await _sut.ProduceAsync(message, cancellationToken);

        // Assert
        await _kafkaProcessorProducerMock.Received(1).ProduceAsync(
            Arg.Is<KafkaProcessorMessage>(km => km.NotificationTargets.Count() == 0),
            cancellationToken);
    }
}
