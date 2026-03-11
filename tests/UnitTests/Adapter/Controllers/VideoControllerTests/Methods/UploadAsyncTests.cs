using Adapter.Controllers.Exceptions;
using Adapter.Presenters.DTOs;
using Domain.Entities;
using Domain.Gateways.Clients.DTOs;
using FluentAssertions;
using NSubstitute;

namespace UnitTests.Adapter.Controllers.VideoControllerTests.Methods;

public class UploadAsyncTests : VideoControllerDependenciesMock
{
    [Fact]
    public async Task When_Valid_Upload_Request_And_Video_Does_Not_Exist_Then_Returns_UploadPresenter()
    {
        // Arrange
        var fileName = "test-video.mp4";
        var contentLength = 1024000L;
        var contentType = "video/mp4";
        var content = new MemoryStream(new byte[] { 1, 2, 3 });
        var userId = "user-123";
        var uploadRequest = new UploadVideoRequest(fileName, contentType, contentLength, content);
        var userRequest = new UserRequest(userId, "John Doe", "recipient@example.com");

        var uploadPath = "/uploads/test-video.mp4";
        var videoId = "video-123";

        _videoUseCaseMock.GetByNameAsync(fileName, userId, Arg.Any<CancellationToken>())
            .Returns((Video?)null);

        _videoUseCaseMock.UploadAsync(Arg.Any<FileUpload>(), Arg.Any<CancellationToken>())
            .Returns(uploadPath);

        _videoUseCaseMock.CreateAsync(Arg.Any<Video>(), Arg.Any<CancellationToken>())
            .Returns(videoId);

        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _sut.UploadAsync(uploadRequest, userRequest, cancellationToken);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task When_Video_With_Same_Name_Already_Exists_Then_Throws_VideoAlrearyCreatedException()
    {
        // Arrange
        var fileName = "existing-video.mp4";
        var contentLength = 1024000L;
        var contentType = "video/mp4";
        var content = new MemoryStream();
        var userId = "user-123";
        var uploadRequest = new UploadVideoRequest(fileName, contentType, contentLength, content);
        var userRequest = new UserRequest(userId, "John Doe", "recipient@example.com");

        var existingVideo = new Video(
            "video-existing",
            DateTime.UtcNow,
            userId,
            "/path",
            fileName,
            contentType,
            contentLength);

        _videoUseCaseMock.GetByNameAsync(fileName, userId, Arg.Any<CancellationToken>())
            .Returns(existingVideo);

        var cancellationToken = CancellationToken.None;

        // Act
        var act = async () => await _sut.UploadAsync(uploadRequest, userRequest, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<VideoAlrearyCreatedException>();
    }

    [Fact]
    public async Task When_GetByNameAsync_Throws_Exception_Then_Propagates_Exception()
    {
        // Arrange
        var uploadRequest = new UploadVideoRequest("test.mp4", "video/mp4", 1024000, new MemoryStream());
        var userRequest = new UserRequest("user-123", "John Doe", "recipient@example.com");
        var expectedException = new InvalidOperationException("Database error");

        _videoUseCaseMock.GetByNameAsync(uploadRequest.FileName, userRequest.Id, Arg.Any<CancellationToken>())
            .Returns(Task.FromException<Video?>(expectedException));

        var cancellationToken = CancellationToken.None;

        // Act
        var act = async () => await _sut.UploadAsync(uploadRequest, userRequest, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task When_UploadAsync_Throws_Exception_Then_Propagates_Exception()
    {
        // Arrange
        var fileName = "test-video.mp4";
        var uploadRequest = new UploadVideoRequest(fileName, "video/mp4", 1024000, new MemoryStream());
        var userRequest = new UserRequest("user-123", "John Doe", "recipient@example.com");
        var expectedException = new IOException("Upload failed");

        _videoUseCaseMock.GetByNameAsync(fileName, userRequest.Id, Arg.Any<CancellationToken>())
            .Returns((Video?)null);

        _videoUseCaseMock.UploadAsync(Arg.Any<FileUpload>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromException<string>(expectedException));

        var cancellationToken = CancellationToken.None;

        // Act
        var act = async () => await _sut.UploadAsync(uploadRequest, userRequest, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<IOException>();
    }

    [Fact]
    public async Task When_CreateAsync_Throws_Exception_Then_Propagates_Exception()
    {
        // Arrange
        var fileName = "test-video.mp4";
        var uploadRequest = new UploadVideoRequest(fileName, "video/mp4", 1024000, new MemoryStream());
        var userRequest = new UserRequest("user-123", "John Doe", "recipient@example.com");
        var expectedException = new InvalidOperationException("Create failed");

        _videoUseCaseMock.GetByNameAsync(fileName, userRequest.Id, Arg.Any<CancellationToken>())
            .Returns((Video?)null);

        _videoUseCaseMock.UploadAsync(Arg.Any<FileUpload>(), Arg.Any<CancellationToken>())
            .Returns("/path");

        _videoUseCaseMock.CreateAsync(Arg.Any<Video>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromException<string>(expectedException));

        var cancellationToken = CancellationToken.None;

        // Act
        var act = async () => await _sut.UploadAsync(uploadRequest, userRequest, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task When_UploadAsync_Called_Then_Calls_UseCase_Methods()
    {
        // Arrange
        var fileName = "test-video.mp4";
        var userId = "user-123";
        var uploadRequest = new UploadVideoRequest(fileName, "video/mp4", 1024000, new MemoryStream());
        var userRequest = new UserRequest(userId, "John Doe", "recipient@example.com");
        var cancellationToken = new CancellationToken();

        _videoUseCaseMock.GetByNameAsync(fileName, userId, cancellationToken)
            .Returns((Video?)null);

        _videoUseCaseMock.UploadAsync(Arg.Any<FileUpload>(), cancellationToken)
            .Returns("/path/test.mp4");

        _videoUseCaseMock.CreateAsync(Arg.Any<Video>(), cancellationToken)
            .Returns("video-123");

        // Act
        await _sut.UploadAsync(uploadRequest, userRequest, cancellationToken);

        // Assert
        await _videoUseCaseMock.Received(1).GetByNameAsync(fileName, userId, cancellationToken);
        await _videoUseCaseMock.Received(1).UploadAsync(Arg.Any<FileUpload>(), cancellationToken);
        await _videoUseCaseMock.Received(1).CreateAsync(Arg.Any<Video>(), cancellationToken);
    }
}
