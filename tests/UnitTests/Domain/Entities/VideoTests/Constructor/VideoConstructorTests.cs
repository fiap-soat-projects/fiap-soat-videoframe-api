using Domain.Entities;
using Domain.Entities.Exceptions;
using FluentAssertions;

namespace UnitTests.Domain.Entities.VideoTests.Constructor;

public class VideoConstructorTests
{
    [Fact]
    public void When_Valid_Parameters_Without_Id_Then_Construction_Succeeds()
    {
        // Arrange
        var userId = "user-123";
        var path = "/videos/test.mp4";
        var name = "test-video.mp4";
        var contentType = "video/mp4";
        var contentLength = 1024000L;

        // Act
        var video = new Video(userId, path, name, contentType, contentLength);

        // Assert
        video.UserId.Should().Be(userId);
        video.Path.Should().Be(path);
        video.Name.Should().Be(name);
        video.ContentType.Should().Be(contentType);
        video.ContentLength.Should().Be(contentLength);
        video.Id.Should().BeNull();
    }

    [Fact]
    public void When_Valid_Parameters_With_Id_Then_Construction_Succeeds()
    {
        // Arrange
        var id = "video-123";
        var createdAt = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc);
        var userId = "user-123";
        var path = "/videos/test.mp4";
        var name = "test-video.mp4";
        var contentType = "video/mp4";
        var contentLength = 1024000L;

        // Act
        var video = new Video(id, createdAt, userId, path, name, contentType, contentLength);

        // Assert
        video.Id.Should().Be(id);
        video.CreatedAt.Should().Be(createdAt);
        video.UserId.Should().Be(userId);
        video.Path.Should().Be(path);
        video.Name.Should().Be(name);
        video.ContentType.Should().Be(contentType);
        video.ContentLength.Should().Be(contentLength);
    }

    [Theory]
    [InlineData("user-1", "/videos/video1.mp4", "video1.mp4", "video/mp4", 2048000)]
    [InlineData("user-abc", "/path/to/video.mkv", "movie.mkv", "video/x-matroska", 5000000)]
    [InlineData("user-xyz-123", "/folder/subfolder/file.avi", "file.avi", "video/x-msvideo", 100000)]
    public void When_Different_Valid_Parameters_Without_Id_Then_Construction_Succeeds(
        string userId, string path, string name, string contentType, long contentLength)
    {
        // Act
        var video = new Video(userId, path, name, contentType, contentLength);

        // Assert
        video.UserId.Should().Be(userId);
        video.Path.Should().Be(path);
        video.Name.Should().Be(name);
        video.ContentType.Should().Be(contentType);
        video.ContentLength.Should().Be(contentLength);
    }

    [Theory]
    [InlineData("id-1", "user-1", "/videos/video1.mp4", "video1.mp4", "video/mp4", 2048000)]
    [InlineData("id-abc", "user-abc", "/path/to/video.mkv", "movie.mkv", "video/x-matroska", 5000000)]
    [InlineData("id-xyz", "user-xyz", "/folder/file.avi", "file.avi", "video/x-msvideo", 100000)]
    public void When_Different_Valid_Parameters_With_Id_Then_Construction_Succeeds(
        string id, string userId, string path, string name, string contentType, long contentLength)
    {
        // Arrange
        var createdAt = DateTime.UtcNow;

        // Act
        var video = new Video(id, createdAt, userId, path, name, contentType, contentLength);

        // Assert
        video.Id.Should().Be(id);
        video.UserId.Should().Be(userId);
        video.Path.Should().Be(path);
        video.Name.Should().Be(name);
        video.ContentType.Should().Be(contentType);
        video.ContentLength.Should().Be(contentLength);
    }

    [Fact]
    public void When_UserId_Is_Null_Then_Throw_InvalidEntityPropertyException()
    {
        // Arrange
        string? nullUserId = null;

        // Act
        var act = () => new Video(nullUserId, "/videos/test.mp4", "test.mp4", "video/mp4", 1024000);

        // Assert
        act.Should()
            .Throw<InvalidEntityPropertyException<Video>>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public void When_UserId_Is_Whitespace_Then_Throw_InvalidEntityPropertyException(string whitespaceUserId)
    {
        // Act
        var act = () => new Video(whitespaceUserId, "/videos/test.mp4", "test.mp4", "video/mp4", 1024000);

        // Assert
        act.Should()
            .Throw<InvalidEntityPropertyException<Video>>();
    }

    [Fact]
    public void When_Path_Is_Null_Then_Throw_InvalidEntityPropertyException()
    {
        // Arrange
        string? nullPath = null;

        // Act
        var act = () => new Video("user-123", nullPath, "test.mp4", "video/mp4", 1024000);

        // Assert
        act.Should()
            .Throw<InvalidEntityPropertyException<Video>>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public void When_Path_Is_Whitespace_Then_Throw_InvalidEntityPropertyException(string whitespacePath)
    {
        // Act
        var act = () => new Video("user-123", whitespacePath, "test.mp4", "video/mp4", 1024000);

        // Assert
        act.Should()
            .Throw<InvalidEntityPropertyException<Video>>();
    }

    [Fact]
    public void When_Name_Is_Null_Then_Throw_InvalidEntityPropertyException()
    {
        // Arrange
        string? nullName = null;

        // Act
        var act = () => new Video("user-123", "/videos/test.mp4", nullName, "video/mp4", 1024000);

        // Assert
        act.Should()
            .Throw<InvalidEntityPropertyException<Video>>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public void When_Name_Is_Whitespace_Then_Throw_InvalidEntityPropertyException(string whitespaceName)
    {
        // Act
        var act = () => new Video("user-123", "/videos/test.mp4", whitespaceName, "video/mp4", 1024000);

        // Assert
        act.Should()
            .Throw<InvalidEntityPropertyException<Video>>();
    }

    [Fact]
    public void When_ContentType_Is_Null_Then_Throw_InvalidEntityPropertyException()
    {
        // Arrange
        string? nullContentType = null;

        // Act
        var act = () => new Video("user-123", "/videos/test.mp4", "test.mp4", nullContentType, 1024000);

        // Assert
        act.Should()
            .Throw<InvalidEntityPropertyException<Video>>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public void When_ContentType_Is_Whitespace_Then_Throw_InvalidEntityPropertyException(string whitespaceContentType)
    {
        // Act
        var act = () => new Video("user-123", "/videos/test.mp4", "test.mp4", whitespaceContentType, 1024000);

        // Assert
        act.Should()
            .Throw<InvalidEntityPropertyException<Video>>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-1000)]
    public void When_ContentLength_Is_Zero_Or_Lower_Then_Throw_InvalidEntityPropertyException(long invalidContentLength)
    {
        // Act
        var act = () => new Video("user-123", "/videos/test.mp4", "test.mp4", "video/mp4", invalidContentLength);

        // Assert
        act.Should()
            .Throw<InvalidEntityPropertyException<Video>>();
    }

    [Fact]
    public void When_Id_Is_Null_Then_Throw_InvalidEntityPropertyException_With_Full_Constructor()
    {
        // Arrange
        string? nullId = null;

        // Act
        var act = () => new Video(nullId, DateTime.UtcNow, "user-123", "/videos/test.mp4", "test.mp4", "video/mp4", 1024000);

        // Assert
        act.Should()
            .Throw<InvalidEntityPropertyException<Video>>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public void When_Id_Is_Whitespace_Then_Throw_InvalidEntityPropertyException_With_Full_Constructor(string whitespaceId)
    {
        // Act
        var act = () => new Video(whitespaceId, DateTime.UtcNow, "user-123", "/videos/test.mp4", "test.mp4", "video/mp4", 1024000);

        // Assert
        act.Should()
            .Throw<InvalidEntityPropertyException<Video>>();
    }
}
