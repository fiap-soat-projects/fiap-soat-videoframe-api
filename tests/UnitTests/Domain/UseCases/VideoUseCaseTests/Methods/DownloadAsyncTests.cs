using FluentAssertions;
using NSubstitute;

namespace UnitTests.Domain.UseCases.VideoUseCaseTests.Methods;

public class DownloadAsyncTests : VideoUseCaseDependenciesMock
{
    [Fact]
    public async Task When_DownloadAsync_Then_Returns_Stream_From_BucketClient()
    {
        // Arrange
        var path = "videos/video-123.mp4";
        var expectedStream = new MemoryStream(new byte[] { 1, 2, 3, 4, 5 });

        _bucketClient.DownloadFileAsync(path, Arg.Any<CancellationToken>()).Returns(expectedStream);

        // Act
        var result = await _sut.DownloadAsync(path, CancellationToken.None);

        // Assert
        result.Should().BeSameAs(expectedStream);
        await _bucketClient.Received(1).DownloadFileAsync(path, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task When_BucketClient_Throws_Exception_Then_Propagate_Exception()
    {
        // Arrange
        var path = "videos/video-123.mp4";
        var expectedException = new IOException("File not found");

        _bucketClient
            .DownloadFileAsync(path, Arg.Any<CancellationToken>())
            .Returns(Task.FromException<Stream>(expectedException));

        // Act
        var act = async () => await _sut.DownloadAsync(path, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<IOException>()
            .WithMessage("File not found");
    }
}
