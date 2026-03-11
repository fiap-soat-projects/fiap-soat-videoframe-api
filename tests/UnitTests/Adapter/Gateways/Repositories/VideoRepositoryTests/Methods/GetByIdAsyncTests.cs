using Domain.Entities;
using FluentAssertions;
using Infrastructure.Entities;
using NSubstitute;

namespace UnitTests.Adapter.Gateways.Repositories.VideoRepositoryTests.Methods;

public class GetByIdAsyncTests : VideoRepositoryDependenciesMock
{
    [Fact]
    public async Task When_Video_Exists_Then_Returns_Domain_Video()
    {
        // Arrange
        var id = "video-123";
        var userId = "user-456";
        var cancellationToken = CancellationToken.None;

        var video = new Video(
            id,
            DateTime.UtcNow,
            userId,
            "/path/to/video.mp4",
            "video.mp4",
            "video/mp4",
            1024000
        );

        var mongoDbEntity = new VideoMongoDb(video)
        {
            Id = id
        };

        _videoMongoDbRepositoryMock.GetByIdAsync(id, userId, cancellationToken)
            .Returns(mongoDbEntity);

        // Act
        var result = await _sut.GetByIdAsync(id, userId, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Video>();
        result!.Id.Should().Be(id);
        result.UserId.Should().Be(userId);
    }

    [Fact]
    public async Task When_Video_Not_Found_Then_Returns_Null()
    {
        // Arrange
        var id = "non-existent-id";
        var userId = "user-456";
        var cancellationToken = CancellationToken.None;

        _videoMongoDbRepositoryMock.GetByIdAsync(id, userId, cancellationToken)
            .Returns((VideoMongoDb?)null);

        // Act
        var result = await _sut.GetByIdAsync(id, userId, cancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task When_GetByIdAsync_Called_Then_Calls_Repository_GetByIdAsync()
    {
        // Arrange
        var id = "video-123";
        var userId = "user-456";
        var cancellationToken = new CancellationToken();

        _videoMongoDbRepositoryMock.GetByIdAsync(id, userId, cancellationToken)
            .Returns((VideoMongoDb?)null);

        // Act
        await _sut.GetByIdAsync(id, userId, cancellationToken);

        // Assert
        await _videoMongoDbRepositoryMock.Received(1).GetByIdAsync(id, userId, cancellationToken);
    }

    [Theory]
    [InlineData("video-1", "user-1")]
    [InlineData("video-abc", "user-xyz")]
    [InlineData("video-999", "user-000")]
    public async Task When_Different_Id_And_UserId_Then_Calls_Repository_With_Correct_Parameters(string id, string userId)
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        var video = new Video(
            id,
            DateTime.UtcNow,
            userId,
            "/path/to/video.mp4",
            "video.mp4",
            "video/mp4",
            1024000
        );

        var mongoDbEntity = new VideoMongoDb(video)
        {
            Id = id
        };

        _videoMongoDbRepositoryMock.GetByIdAsync(id, userId, cancellationToken)
            .Returns(mongoDbEntity);

        // Act
        var result = await _sut.GetByIdAsync(id, userId, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        result.UserId.Should().Be(userId);
        await _videoMongoDbRepositoryMock.Received(1).GetByIdAsync(id, userId, cancellationToken);
    }

    [Fact]
    public async Task When_Valid_Entity_Then_Converts_To_Domain_Correctly()
    {
        // Arrange
        var id = "video-123";
        var userId = "user-456";
        var path = "/path/to/video.mp4";
        var name = "video.mp4";
        var contentType = "video/mp4";
        var contentLength = 1024000L;
        var cancellationToken = CancellationToken.None;

        var video = new Video(
            id,
            DateTime.UtcNow,
            userId,
            path,
            name,
            contentType,
            contentLength
        );

        var mongoDbEntity = new VideoMongoDb(video)
        {
            Id = id
        };

        _videoMongoDbRepositoryMock.GetByIdAsync(id, userId, cancellationToken)
            .Returns(mongoDbEntity);

        // Act
        var result = await _sut.GetByIdAsync(id, userId, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        result.UserId.Should().Be(userId);
        result.Path.Should().Be(path);
        result.Name.Should().Be(name);
        result.ContentType.Should().Be(contentType);
        result.ContentLength.Should().Be(contentLength);
    }
}
