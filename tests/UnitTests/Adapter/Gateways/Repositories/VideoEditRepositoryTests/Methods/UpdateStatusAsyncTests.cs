using Domain.Entities.Enums;
using FluentAssertions;
using NSubstitute;

namespace UnitTests.Adapter.Gateways.Repositories.VideoEditRepositoryTests.Methods;

public class UpdateStatusAsyncTests : VideoEditRepositoryDependenciesMock
{
    [Fact]
    public async Task When_Valid_Parameters_Then_Calls_Repository_UpdateStatusAsync()
    {
        // Arrange
        var id = "edit-123";
        var userId = "user-456";
        var status = EditStatus.Processing;
        var cancellationToken = CancellationToken.None;

        // Act
        await _sut.UpdateStatusAsync(id, userId, status, cancellationToken);

        // Assert
        await _videoEditMongoDbRepositoryMock.Received(1).UpdateStatusAsync(id, userId, "Processing", cancellationToken);
    }

    [Theory]
    [InlineData(EditStatus.Created, "Created")]
    [InlineData(EditStatus.Processing, "Processing")]
    [InlineData(EditStatus.Processed, "Processed")]
    [InlineData(EditStatus.Sending, "Sending")]
    [InlineData(EditStatus.Sent, "Sent")]
    [InlineData(EditStatus.Error, "Error")]
    public async Task When_Different_Status_Then_Converts_To_String_Correctly(EditStatus status, string expectedStatusString)
    {
        // Arrange
        var id = "edit-123";
        var userId = "user-456";
        var cancellationToken = CancellationToken.None;

        // Act
        await _sut.UpdateStatusAsync(id, userId, status, cancellationToken);

        // Assert
        await _videoEditMongoDbRepositoryMock.Received(1).UpdateStatusAsync(id, userId, expectedStatusString, cancellationToken);
    }

    [Theory]
    [InlineData("edit-1", "user-1", EditStatus.Created)]
    [InlineData("edit-abc", "user-xyz", EditStatus.Processing)]
    [InlineData("edit-999", "user-000", EditStatus.Processed)]
    public async Task When_Different_Parameters_Then_Calls_Repository_With_Correct_Values(string id, string userId, EditStatus status)
    {
        // Arrange
        var cancellationToken = new CancellationToken();

        // Act
        await _sut.UpdateStatusAsync(id, userId, status, cancellationToken);

        // Assert
        await _videoEditMongoDbRepositoryMock.Received(1).UpdateStatusAsync(id, userId, status.ToString(), cancellationToken);
    }

    [Fact]
    public async Task When_UpdateStatusAsync_Called_Then_Completes_Successfully()
    {
        // Arrange
        var id = "edit-123";
        var userId = "user-456";
        var status = EditStatus.Sent;
        var cancellationToken = CancellationToken.None;

        _videoEditMongoDbRepositoryMock.UpdateStatusAsync(id, userId, status.ToString(), cancellationToken)
            .Returns(Task.CompletedTask);

        // Act
        var act = async () => await _sut.UpdateStatusAsync(id, userId, status, cancellationToken);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_Status_Is_None_Then_Converts_To_None_String()
    {
        // Arrange
        var id = "edit-123";
        var userId = "user-456";
        var status = EditStatus.None;
        var cancellationToken = CancellationToken.None;

        // Act
        await _sut.UpdateStatusAsync(id, userId, status, cancellationToken);

        // Assert
        await _videoEditMongoDbRepositoryMock.Received(1).UpdateStatusAsync(id, userId, "None", cancellationToken);
    }
}
