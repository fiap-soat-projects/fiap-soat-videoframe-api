using Adapter.Presenters.DTOs;
using Domain.Entities;
using Domain.Entities.Enums;
using FluentAssertions;
using NSubstitute;

namespace UnitTests.Adapter.Controllers.VideoEditControllerTests.Methods;

public class UpdateStatusAsyncTests : VideoEditControllerDependenciesMock
{
    [Fact]
    public async Task When_Valid_Id_Status_And_UserRequest_Then_Calls_UseCase()
    {
        // Arrange
        var id = "edit-123";
        var status = EditStatus.Processed;
        var userId = "user-123";
        var userRequest = new UserRequest(userId, "John Doe", "recipient@example.com");
        var cancellationToken = CancellationToken.None;

        _videoEditUseCaseMock.UpdateStatusAsync(id, status, userId, cancellationToken)
            .Returns(Task.CompletedTask);

        // Act
        await _sut.UpdateStatusAsync(id, status, userRequest, cancellationToken);

        // Assert
        await _videoEditUseCaseMock.Received(1).UpdateStatusAsync(id, status, userId, cancellationToken);
    }

    [Theory]
    [InlineData(EditStatus.Created)]
    [InlineData(EditStatus.Processing)]
    [InlineData(EditStatus.Processed)]
    [InlineData(EditStatus.Error)]
    public async Task When_Different_Statuses_Then_Calls_UseCase_With_Correct_Status(EditStatus status)
    {
        // Arrange
        var id = "edit-123";
        var userId = "user-123";
        var userRequest = new UserRequest(userId, "John Doe", "recipient@example.com");
        var cancellationToken = new CancellationToken();

        _videoEditUseCaseMock.UpdateStatusAsync(id, status, userId, cancellationToken)
            .Returns(Task.CompletedTask);

        // Act
        await _sut.UpdateStatusAsync(id, status, userRequest, cancellationToken);

        // Assert
        await _videoEditUseCaseMock.Received(1).UpdateStatusAsync(id, status, userId, cancellationToken);
    }

    [Fact]
    public async Task When_UseCase_Throws_Exception_Then_Propagates_Exception()
    {
        // Arrange
        var id = "edit-123";
        var status = EditStatus.Processed;
        var userId = "user-123";
        var userRequest = new UserRequest(userId, "John Doe", "recipient@example.com");
        var expectedException = new InvalidOperationException("Update failed");

        _videoEditUseCaseMock.UpdateStatusAsync(id, status, userId, Arg.Any<CancellationToken>())
            .Returns(Task.FromException(expectedException));

        var cancellationToken = CancellationToken.None;

        // Act
        var act = async () => await _sut.UpdateStatusAsync(id, status, userRequest, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
