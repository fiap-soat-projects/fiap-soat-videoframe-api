using Domain.Entities;
using Domain.Entities.Exceptions;
using FluentAssertions;
using NSubstitute;

namespace UnitTests.Domain.UseCases.VideoUseCaseTests.Methods;

public class GetLinkAsyncTests : VideoUseCaseDependenciesMock
{
    [Fact]
    public async Task When_Video_Found_Then_Returns_PresignedUrl()
    {
        // Arrange
        var id = "video-123";
        var userId = "user-123";
        var videoPath = "videos/video-123.mp4";
        var expectedUrl = "https://presigned.url/video-123.mp4";

        var video = new Video(id, DateTime.UtcNow, userId, videoPath, "video.mp4", "video/mp4", 1024);

        _videoRepository.GetByIdAsync(id, userId, Arg.Any<CancellationToken>()).Returns(video);
        _bucketClient.GetPreSignedDownloadUrlAsync(videoPath, Arg.Any<CancellationToken>()).Returns(expectedUrl);

        // Act
        var result = await _sut.GetLinkAsync(id, userId, CancellationToken.None);

        // Assert
        result.Should().Be(expectedUrl);
        await _videoRepository.Received(1).GetByIdAsync(id, userId, Arg.Any<CancellationToken>());
        await _bucketClient.Received(1).GetPreSignedDownloadUrlAsync(videoPath, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task When_Video_Not_Found_Then_Throw_EntityNotFoundException()
    {
        // Arrange
        var id = "not-found";
        var userId = "user-123";

        _videoRepository.GetByIdAsync(id, userId, Arg.Any<CancellationToken>()).Returns((Video?)null);

        // Act
        var act = async () => await _sut.GetLinkAsync(id, userId, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<Video>>();
        await _bucketClient.DidNotReceive().GetPreSignedDownloadUrlAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task When_BucketClient_Throws_Exception_Then_Propagate_Exception()
    {
        // Arrange
        var id = "video-123";
        var userId = "user-123";
        var videoPath = "videos/video-123.mp4";
        var expectedException = new IOException("S3 error");

        var video = new Video(id, DateTime.UtcNow, userId, videoPath, "video.mp4", "video/mp4", 1024);

        _videoRepository.GetByIdAsync(id, userId, Arg.Any<CancellationToken>()).Returns(video);
        _bucketClient
            .GetPreSignedDownloadUrlAsync(videoPath, Arg.Any<CancellationToken>())
            .Returns(Task.FromException<string>(expectedException));

        // Act
        var act = async () => await _sut.GetLinkAsync(id, userId, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<IOException>()
            .WithMessage("S3 error");
    }

    [Fact]
    public async Task When_Repository_Throws_Exception_Then_Propagate_Exception()
    {
        // Arrange
        var id = "error-id";
        var userId = "user-123";
        var expectedException = new InvalidOperationException("Database error");

        _videoRepository
            .GetByIdAsync(id, userId, Arg.Any<CancellationToken>())
            .Returns(Task.FromException<Video?>(expectedException));

        // Act
        var act = async () => await _sut.GetLinkAsync(id, userId, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Database error");
        await _bucketClient.DidNotReceive().GetPreSignedDownloadUrlAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
    }
}
