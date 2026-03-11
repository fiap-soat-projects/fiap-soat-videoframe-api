using Domain.Entities;
using Domain.Entities.Enums;
using FluentAssertions;
using Infrastructure.Entities;
using NSubstitute;

namespace UnitTests.Adapter.Gateways.Repositories.VideoEditRepositoryTests.Methods;

public class GetByIdAsyncTests : VideoEditRepositoryDependenciesMock
{
    [Fact]
    public async Task When_VideoEdit_Exists_Then_Returns_Domain_VideoEdit()
    {
        // Arrange
        var id = "edit-123";
        var userId = "user-456";
        var cancellationToken = CancellationToken.None;

        var videoEdit = new VideoEdit(
            id,
            DateTime.UtcNow,
            userId,
            "recipient@example.com",
            EditType.Frame,
            EditStatus.Created,
            "video-1",
            "/path/to/edit",
            []
        );

        var mongoDbEntity = new VideoEditMongoDb(videoEdit)
        {
            Id = id
        };

        _videoEditMongoDbRepositoryMock.GetByIdAsync(id, userId, cancellationToken)
            .Returns(mongoDbEntity);

        // Act
        var result = await _sut.GetByIdAsync(id, userId, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<VideoEdit>();
        result!.Id.Should().Be(id);
        result.UserId.Should().Be(userId);
    }

    [Fact]
    public async Task When_VideoEdit_Not_Found_Then_Returns_Null()
    {
        // Arrange
        var id = "non-existent-id";
        var userId = "user-456";
        var cancellationToken = CancellationToken.None;

        _videoEditMongoDbRepositoryMock.GetByIdAsync(id, userId, cancellationToken)
            .Returns((VideoEditMongoDb?)null);

        // Act
        var result = await _sut.GetByIdAsync(id, userId, cancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task When_GetByIdAsync_Called_Then_Calls_Repository_GetByIdAsync()
    {
        // Arrange
        var id = "edit-123";
        var userId = "user-456";
        var cancellationToken = new CancellationToken();

        _videoEditMongoDbRepositoryMock.GetByIdAsync(id, userId, cancellationToken)
            .Returns((VideoEditMongoDb?)null);

        // Act
        await _sut.GetByIdAsync(id, userId, cancellationToken);

        // Assert
        await _videoEditMongoDbRepositoryMock.Received(1).GetByIdAsync(id, userId, cancellationToken);
    }

    [Theory]
    [InlineData("edit-1", "user-1")]
    [InlineData("edit-abc", "user-xyz")]
    [InlineData("edit-999", "user-000")]
    public async Task When_Different_Id_And_UserId_Then_Calls_Repository_With_Correct_Parameters(string id, string userId)
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        var videoEdit = new VideoEdit(
            id,
            DateTime.UtcNow,
            userId,
            "recipient@example.com",
            EditType.Frame,
            EditStatus.Created,
            "video-1",
            "/path/to/edit",
            []
        );

        var mongoDbEntity = new VideoEditMongoDb(videoEdit)
        {
            Id = id
        };

        _videoEditMongoDbRepositoryMock.GetByIdAsync(id, userId, cancellationToken)
            .Returns(mongoDbEntity);

        // Act
        var result = await _sut.GetByIdAsync(id, userId, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        result.UserId.Should().Be(userId);
        await _videoEditMongoDbRepositoryMock.Received(1).GetByIdAsync(id, userId, cancellationToken);
    }

    [Fact]
    public async Task When_Valid_Entity_Then_Converts_To_Domain_Correctly()
    {
        // Arrange
        var id = "edit-123";
        var userId = "user-456";
        var recipient = "recipient@example.com";
        var videoId = "video-789";
        var editPath = "/path/to/edit";
        var cancellationToken = CancellationToken.None;

        var videoEdit = new VideoEdit(
            id,
            DateTime.UtcNow,
            userId,
            recipient,
            EditType.Frame,
            EditStatus.Processed,
            videoId,
            editPath,
            []
        );

        var mongoDbEntity = new VideoEditMongoDb(videoEdit)
        {
            Id = id
        };

        _videoEditMongoDbRepositoryMock.GetByIdAsync(id, userId, cancellationToken)
            .Returns(mongoDbEntity);

        // Act
        var result = await _sut.GetByIdAsync(id, userId, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        result.UserId.Should().Be(userId);
        result.Recipient.Should().Be(recipient);
        result.VideoId.Should().Be(videoId);
        result.EditPath.Should().Be(editPath);
        result.Type.Should().Be(EditType.Frame);
        result.Status.Should().Be(EditStatus.Processed);
    }
}
