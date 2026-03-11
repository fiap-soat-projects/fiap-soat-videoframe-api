using Domain.Entities;
using FluentAssertions;
using Infrastructure.Entities;
using NSubstitute;

namespace UnitTests.Adapter.Gateways.Repositories.VideoRepositoryTests.Methods;

public class InsertOneAsyncTests : VideoRepositoryDependenciesMock
{
    [Fact]
    public async Task When_Valid_Video_Then_Returns_Inserted_Id()
    {
        // Arrange
        var video = new Video(
            "video-123",
            DateTime.UtcNow,
            "user-456",
            "/path/to/video.mp4",
            "video.mp4",
            "video/mp4",
            1024000
        );

        var expectedId = "inserted-id-789";
        var cancellationToken = CancellationToken.None;

        _videoMongoDbRepositoryMock.InsertOneAsync(Arg.Any<VideoMongoDb>(), cancellationToken)
            .Returns(expectedId);

        // Act
        var result = await _sut.InsertOneAsync(video, cancellationToken);

        // Assert
        result.Should().Be(expectedId);
    }

    [Fact]
    public async Task When_InsertOneAsync_Called_Then_Calls_Repository_InsertOneAsync()
    {
        // Arrange
        var video = new Video(
            "video-123",
            DateTime.UtcNow,
            "user-456",
            "/path/to/video.mp4",
            "video.mp4",
            "video/mp4",
            1024000
        );

        var cancellationToken = new CancellationToken();

        _videoMongoDbRepositoryMock.InsertOneAsync(Arg.Any<VideoMongoDb>(), cancellationToken)
            .Returns("inserted-id");

        // Act
        await _sut.InsertOneAsync(video, cancellationToken);

        // Assert
        await _videoMongoDbRepositoryMock.Received(1).InsertOneAsync(Arg.Any<VideoMongoDb>(), cancellationToken);
    }

    [Fact]
    public async Task When_InsertOneAsync_Called_Then_Converts_Domain_To_MongoDb_Entity()
    {
        // Arrange
        var video = new Video(
            "video-123",
            DateTime.UtcNow,
            "user-456",
            "/path/to/video.mp4",
            "video.mp4",
            "video/mp4",
            1024000
        );

        var cancellationToken = CancellationToken.None;
        VideoMongoDb? capturedEntity = null;

        _videoMongoDbRepositoryMock.InsertOneAsync(Arg.Do<VideoMongoDb>(e => capturedEntity = e), cancellationToken)
            .Returns("inserted-id");

        // Act
        await _sut.InsertOneAsync(video, cancellationToken);

        // Assert
        capturedEntity.Should().NotBeNull();
        capturedEntity!.UserId.Should().Be(video.UserId);
        capturedEntity.Path.Should().Be(video.Path);
        capturedEntity.Name.Should().Be(video.Name);
        capturedEntity.ContentType.Should().Be(video.ContentType);
        capturedEntity.ContentLength.Should().Be(video.ContentLength);
    }

    [Theory]
    [InlineData("user-1", "/path/1", "video1.mp4", "video/mp4", 1024000)]
    [InlineData("user-abc", "/path/abc", "movie.avi", "video/x-msvideo", 2048000)]
    [InlineData("user-xyz", "/path/xyz", "file.mkv", "video/x-matroska", 5000000)]
    public async Task When_Different_Videos_Then_Inserts_Correctly(string userId, string path, string name, string contentType, long contentLength)
    {
        // Arrange
        var video = new Video(
            "video-123",
            DateTime.UtcNow,
            userId,
            path,
            name,
            contentType,
            contentLength
        );

        var cancellationToken = CancellationToken.None;

        _videoMongoDbRepositoryMock.InsertOneAsync(Arg.Any<VideoMongoDb>(), cancellationToken)
            .Returns("inserted-id");

        // Act
        var result = await _sut.InsertOneAsync(video, cancellationToken);

        // Assert
        result.Should().NotBeNullOrEmpty();
        await _videoMongoDbRepositoryMock.Received(1).InsertOneAsync(Arg.Any<VideoMongoDb>(), cancellationToken);
    }

    [Fact]
    public async Task When_Video_With_Different_ContentType_Then_Converts_Correctly()
    {
        // Arrange
        var video = new Video(
            "video-123",
            DateTime.UtcNow,
            "user-456",
            "/path/to/video.webm",
            "video.webm",
            "video/webm",
            3000000
        );

        var cancellationToken = CancellationToken.None;
        VideoMongoDb? capturedEntity = null;

        _videoMongoDbRepositoryMock.InsertOneAsync(Arg.Do<VideoMongoDb>(e => capturedEntity = e), cancellationToken)
            .Returns("inserted-id");

        // Act
        await _sut.InsertOneAsync(video, cancellationToken);

        // Assert
        capturedEntity.Should().NotBeNull();
        capturedEntity!.ContentType.Should().Be("video/webm");
    }

    [Fact]
    public async Task When_Video_With_Different_ContentLength_Then_Converts_Correctly()
    {
        // Arrange
        var contentLength = 9999999L;
        var video = new Video(
            "video-123",
            DateTime.UtcNow,
            "user-456",
            "/path/to/video.mp4",
            "video.mp4",
            "video/mp4",
            contentLength
        );

        var cancellationToken = CancellationToken.None;
        VideoMongoDb? capturedEntity = null;

        _videoMongoDbRepositoryMock.InsertOneAsync(Arg.Do<VideoMongoDb>(e => capturedEntity = e), cancellationToken)
            .Returns("inserted-id");

        // Act
        await _sut.InsertOneAsync(video, cancellationToken);

        // Assert
        capturedEntity.Should().NotBeNull();
        capturedEntity!.ContentLength.Should().Be(contentLength);
    }
}
