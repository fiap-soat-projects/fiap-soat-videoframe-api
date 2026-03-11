using FluentAssertions;
using NSubstitute;

namespace UnitTests.Adapter.Gateways.Repositories.VideoRepositoryTests.Methods;

public class DeleteAsyncTests : VideoRepositoryDependenciesMock
{
    [Fact]
    public async Task When_Valid_Parameters_Then_Calls_Repository_DeleteAsync()
    {
        // Arrange
        var id = "video-123";
        var userId = "user-456";
        var cancellationToken = CancellationToken.None;

        // Act
        await _sut.DeleteAsync(id, userId, cancellationToken);

        // Assert
        await _videoMongoDbRepositoryMock.Received(1).DeleteAsync(id, userId, cancellationToken);
    }

    [Theory]
    [InlineData("video-1", "user-1")]
    [InlineData("video-abc", "user-xyz")]
    [InlineData("video-999", "user-000")]
    public async Task When_Different_Id_And_UserId_Then_Calls_Repository_With_Correct_Parameters(string id, string userId)
    {
        // Arrange
        var cancellationToken = new CancellationToken();

        // Act
        await _sut.DeleteAsync(id, userId, cancellationToken);

        // Assert
        await _videoMongoDbRepositoryMock.Received(1).DeleteAsync(id, userId, cancellationToken);
    }

    [Fact]
    public async Task When_DeleteAsync_Called_Then_Completes_Successfully()
    {
        // Arrange
        var id = "video-123";
        var userId = "user-456";
        var cancellationToken = CancellationToken.None;

        _videoMongoDbRepositoryMock.DeleteAsync(id, userId, cancellationToken)
            .Returns(Task.CompletedTask);

        // Act
        var act = async () => await _sut.DeleteAsync(id, userId, cancellationToken);

        // Assert
        await act.Should().NotThrowAsync();
    }
}
