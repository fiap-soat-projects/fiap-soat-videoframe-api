using FluentAssertions;
using NSubstitute;

namespace UnitTests.Adapter.Gateways.Clients.BucketClientTests.Methods;

public class DownloadFileAsyncTests : BucketClientDependenciesMock
{
    [Fact]
    public async Task When_Valid_FilePath_Then_Returns_Stream()
    {
        // Arrange
        var filePath = "users/user-123/video/video.mp4";
        var expectedStream = new MemoryStream(new byte[] { 1, 2, 3 });
        var cancellationToken = CancellationToken.None;

        _s3BucketClientMock.DownloadAsync(filePath, cancellationToken)
            .Returns(expectedStream);

        // Act
        var result = await _sut.DownloadFileAsync(filePath, cancellationToken);

        // Assert
        result.Should().BeSameAs(expectedStream);
    }

    [Fact]
    public async Task When_Download_Called_Then_Calls_S3_With_Correct_Path()
    {
        // Arrange
        var filePath = "users/user-123/video/video.mp4";
        var expectedStream = new MemoryStream();
        var cancellationToken = new CancellationToken();

        _s3BucketClientMock.DownloadAsync(filePath, cancellationToken)
            .Returns(expectedStream);

        // Act
        await _sut.DownloadFileAsync(filePath, cancellationToken);

        // Assert
        await _s3BucketClientMock.Received(1).DownloadAsync(filePath, cancellationToken);
    }

    [Theory]
    [InlineData("users/user-1/video/video1.mp4")]
    [InlineData("users/user-abc/video/movie.avi")]
    [InlineData("users/user-xyz-123/edit/edit.zip")]
    public async Task When_Different_Paths_Then_Returns_Correct_Streams(string filePath)
    {
        // Arrange
        var expectedStream = new MemoryStream();
        var cancellationToken = CancellationToken.None;

        _s3BucketClientMock.DownloadAsync(filePath, cancellationToken)
            .Returns(expectedStream);

        // Act
        var result = await _sut.DownloadFileAsync(filePath, cancellationToken);

        // Assert
        result.Should().BeSameAs(expectedStream);
    }

    [Fact]
    public async Task When_S3_Download_Throws_Exception_Then_Propagates_Exception()
    {
        // Arrange
        var filePath = "users/user-123/video/video.mp4";
        var expectedException = new IOException("Download failed");
        var cancellationToken = CancellationToken.None;

        _s3BucketClientMock.DownloadAsync(filePath, cancellationToken)
            .Returns(Task.FromException<Stream>(expectedException));

        // Act
        var act = async () => await _sut.DownloadFileAsync(filePath, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<IOException>();
    }

    [Fact]
    public async Task When_Stream_Is_Empty_Then_Returns_Empty_Stream()
    {
        // Arrange
        var filePath = "users/user-123/video/video.mp4";
        var emptyStream = new MemoryStream();
        var cancellationToken = CancellationToken.None;

        _s3BucketClientMock.DownloadAsync(filePath, cancellationToken)
            .Returns(emptyStream);

        // Act
        var result = await _sut.DownloadFileAsync(filePath, cancellationToken);

        // Assert
        result.Should().BeSameAs(emptyStream);
        result.Length.Should().Be(0);
    }
}
