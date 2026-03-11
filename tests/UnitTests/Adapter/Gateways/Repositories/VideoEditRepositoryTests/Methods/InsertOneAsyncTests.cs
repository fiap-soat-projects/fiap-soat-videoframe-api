using Domain.Entities;
using Domain.Entities.Enums;
using FluentAssertions;
using Infrastructure.Entities;
using NSubstitute;

namespace UnitTests.Adapter.Gateways.Repositories.VideoEditRepositoryTests.Methods;

public class InsertOneAsyncTests : VideoEditRepositoryDependenciesMock
{
    [Fact]
    public async Task When_Valid_VideoEdit_Then_Returns_Inserted_Id()
    {
        // Arrange
        var videoEdit = new VideoEdit(
            "edit-123",
            DateTime.UtcNow,
            "user-456",
            "recipient@example.com",
            EditType.Frame,
            EditStatus.Created,
            "video-1",
            "/path/to/edit",
            []
        );

        var expectedId = "inserted-id-789";
        var cancellationToken = CancellationToken.None;

        _videoEditMongoDbRepositoryMock.InsertOneAsync(Arg.Any<VideoEditMongoDb>(), cancellationToken)
            .Returns(expectedId);

        // Act
        var result = await _sut.InsertOneAsync(videoEdit, cancellationToken);

        // Assert
        result.Should().Be(expectedId);
    }

    [Fact]
    public async Task When_InsertOneAsync_Called_Then_Calls_Repository_InsertOneAsync()
    {
        // Arrange
        var videoEdit = new VideoEdit(
            "edit-123",
            DateTime.UtcNow,
            "user-456",
            "recipient@example.com",
            EditType.Frame,
            EditStatus.Created,
            "video-1",
            "/path/to/edit",
            []
        );

        var cancellationToken = new CancellationToken();

        _videoEditMongoDbRepositoryMock.InsertOneAsync(Arg.Any<VideoEditMongoDb>(), cancellationToken)
            .Returns("inserted-id");

        // Act
        await _sut.InsertOneAsync(videoEdit, cancellationToken);

        // Assert
        await _videoEditMongoDbRepositoryMock.Received(1).InsertOneAsync(Arg.Any<VideoEditMongoDb>(), cancellationToken);
    }

    [Fact]
    public async Task When_InsertOneAsync_Called_Then_Converts_Domain_To_MongoDb_Entity()
    {
        // Arrange
        var videoEdit = new VideoEdit(
            "edit-123",
            DateTime.UtcNow,
            "user-456",
            "recipient@example.com",
            EditType.Frame,
            EditStatus.Created,
            "video-1",
            "/path/to/edit",
            []
        );

        var cancellationToken = CancellationToken.None;
        VideoEditMongoDb? capturedEntity = null;

        _videoEditMongoDbRepositoryMock.InsertOneAsync(Arg.Do<VideoEditMongoDb>(e => capturedEntity = e), cancellationToken)
            .Returns("inserted-id");

        // Act
        await _sut.InsertOneAsync(videoEdit, cancellationToken);

        // Assert
        capturedEntity.Should().NotBeNull();
        capturedEntity!.UserId.Should().Be(videoEdit.UserId);
        capturedEntity.Recipient.Should().Be(videoEdit.Recipient);
        capturedEntity.Type.Should().Be(videoEdit.Type.ToString());
        capturedEntity.Status.Should().Be(videoEdit.Status.ToString());
        capturedEntity.VideoId.Should().Be(videoEdit.VideoId);
        capturedEntity.EditPath.Should().Be(videoEdit.EditPath);
    }

    [Theory]
    [InlineData("user-1", "recipient1@example.com", "video-1", "/path/1")]
    [InlineData("user-abc", "recipient2@example.com", "video-abc", "/path/abc")]
    [InlineData("user-xyz", "recipient3@example.com", "video-xyz", "/path/xyz")]
    public async Task When_Different_VideoEdits_Then_Inserts_Correctly(string userId, string recipient, string videoId, string editPath)
    {
        // Arrange
        var videoEdit = new VideoEdit(
            "edit-123",
            DateTime.UtcNow,
            userId,
            recipient,
            EditType.Frame,
            EditStatus.Created,
            videoId,
            editPath,
            []
        );

        var cancellationToken = CancellationToken.None;

        _videoEditMongoDbRepositoryMock.InsertOneAsync(Arg.Any<VideoEditMongoDb>(), cancellationToken)
            .Returns("inserted-id");

        // Act
        var result = await _sut.InsertOneAsync(videoEdit, cancellationToken);

        // Assert
        result.Should().NotBeNullOrEmpty();
        await _videoEditMongoDbRepositoryMock.Received(1).InsertOneAsync(Arg.Any<VideoEditMongoDb>(), cancellationToken);
    }

    [Fact]
    public async Task When_VideoEdit_With_Different_Status_Then_Converts_Status_Correctly()
    {
        // Arrange
        var videoEdit = new VideoEdit(
            "edit-123",
            DateTime.UtcNow,
            "user-456",
            "recipient@example.com",
            EditType.Frame,
            EditStatus.Processing,
            "video-1",
            "/path/to/edit",
            []
        );

        var cancellationToken = CancellationToken.None;
        VideoEditMongoDb? capturedEntity = null;

        _videoEditMongoDbRepositoryMock.InsertOneAsync(Arg.Do<VideoEditMongoDb>(e => capturedEntity = e), cancellationToken)
            .Returns("inserted-id");

        // Act
        await _sut.InsertOneAsync(videoEdit, cancellationToken);

        // Assert
        capturedEntity.Should().NotBeNull();
        capturedEntity!.Status.Should().Be("Processing");
    }

    [Fact]
    public async Task When_VideoEdit_With_Different_Type_Then_Converts_Type_Correctly()
    {
        // Arrange
        var videoEdit = new VideoEdit(
            "edit-123",
            DateTime.UtcNow,
            "user-456",
            "recipient@example.com",
            EditType.Frame,
            EditStatus.Created,
            "video-1",
            "/path/to/edit",
            []
        );

        var cancellationToken = CancellationToken.None;
        VideoEditMongoDb? capturedEntity = null;

        _videoEditMongoDbRepositoryMock.InsertOneAsync(Arg.Do<VideoEditMongoDb>(e => capturedEntity = e), cancellationToken)
            .Returns("inserted-id");

        // Act
        await _sut.InsertOneAsync(videoEdit, cancellationToken);

        // Assert
        capturedEntity.Should().NotBeNull();
        capturedEntity!.Type.Should().Be("Frame");
    }
}
