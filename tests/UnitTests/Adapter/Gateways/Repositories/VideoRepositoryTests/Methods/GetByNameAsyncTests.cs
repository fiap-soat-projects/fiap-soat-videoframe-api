using Domain.Entities;
using FluentAssertions;
using Infrastructure.Entities;
using NSubstitute;

namespace UnitTests.Adapter.Gateways.Repositories.VideoRepositoryTests.Methods;

public class GetByNameAsyncTests : VideoRepositoryDependenciesMock
{
    [Fact]
    public async Task When_Video_Exists_Then_Returns_Domain_Video()
    {
        // Arrange
        var name = "video.mp4";
        var userId = "user-456";
        var cancellationToken = CancellationToken.None;

        var video = new Video(
            "video-123",
            DateTime.UtcNow,
            userId,
            "/path/to/video.mp4",
            name,
            "video/mp4",
            1024000
        );

        var mongoDbEntity = new VideoMongoDb(video)
        {
            Id = "video-123"
        };

        _videoMongoDbRepositoryMock.GetByNameAsync(name, userId, cancellationToken)
            .Returns(mongoDbEntity);

        // Act
        var result = await _sut.GetByNameAsync(name, userId, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Video>();
        result!.Name.Should().Be(name);
        result.UserId.Should().Be(userId);
    }

    [Fact]
    public async Task When_Video_Not_Found_Then_Returns_Null()
    {
        // Arrange
        var name = "non-existent-video.mp4";
        var userId = "user-456";
        var cancellationToken = CancellationToken.None;

        _videoMongoDbRepositoryMock.GetByNameAsync(name, userId, cancellationToken)
            .Returns((VideoMongoDb?)null);

        // Act
        var result = await _sut.GetByNameAsync(name, userId, cancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task When_GetByNameAsync_Called_Then_Calls_Repository_GetByNameAsync()
    {
        // Arrange
        var name = "video.mp4";
        var userId = "user-456";
        var cancellationToken = new CancellationToken();

        _videoMongoDbRepositoryMock.GetByNameAsync(name, userId, cancellationToken)
            .Returns((VideoMongoDb?)null);

        // Act
        await _sut.GetByNameAsync(name, userId, cancellationToken);

        // Assert
        await _videoMongoDbRepositoryMock.Received(1).GetByNameAsync(name, userId, cancellationToken);
    }

    [Theory]
    [InlineData("video1.mp4", "user-1")]
    [InlineData("movie.avi", "user-abc")]
    [InlineData("file.mkv", "user-xyz")]
    public async Task When_Different_Name_And_UserId_Then_Calls_Repository_With_Correct_Parameters(string name, string userId)
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        var video = new Video(
            "video-123",
            DateTime.UtcNow,
            userId,
            "/path/to/" + name,
            name,
            "video/mp4",
            1024000
        );

        var mongoDbEntity = new VideoMongoDb(video)
        {
            Id = "video-123"
        };

        _videoMongoDbRepositoryMock.GetByNameAsync(name, userId, cancellationToken)
            .Returns(mongoDbEntity);

        // Act
        var result = await _sut.GetByNameAsync(name, userId, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be(name);
        result.UserId.Should().Be(userId);
        await _videoMongoDbRepositoryMock.Received(1).GetByNameAsync(name, userId, cancellationToken);
    }

    [Fact]
    public async Task When_Valid_Entity_Then_Converts_To_Domain_Correctly()
    {
        // Arrange
        var name = "video.mp4";
        var userId = "user-456";
        var id = "video-123";
        var path = "/path/to/video.mp4";
        var contentType = "video/mp4";
        var contentLength = 2048000L;
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

        _videoMongoDbRepositoryMock.GetByNameAsync(name, userId, cancellationToken)
            .Returns(mongoDbEntity);

        // Act
        var result = await _sut.GetByNameAsync(name, userId, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        result.Name.Should().Be(name);
        result.UserId.Should().Be(userId);
        result.Path.Should().Be(path);
        result.ContentType.Should().Be(contentType);
        result.ContentLength.Should().Be(contentLength);
    }
}
