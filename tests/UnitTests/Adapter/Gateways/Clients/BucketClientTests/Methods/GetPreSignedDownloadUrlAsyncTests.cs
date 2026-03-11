using FluentAssertions;
using NSubstitute;

namespace UnitTests.Adapter.Gateways.Clients.BucketClientTests.Methods;

public class GetPreSignedDownloadUrlAsyncTests : BucketClientDependenciesMock
{
    [Fact]
    public async Task When_Valid_FilePath_Then_Returns_PreSignedUrl()
    {
        // Arrange
        var filePath = "users/user-123/video/video.mp4";
        var expectedUrl = "https://s3.amazonaws.com/bucket/users/user-123/video/video.mp4?AWSAccessKeyId=...";
        var cancellationToken = CancellationToken.None;

        _s3BucketClientMock.GetPreSignedDownloadUrlAsync(filePath, cancellationToken)
            .Returns(expectedUrl);

        // Act
        var result = await _sut.GetPreSignedDownloadUrlAsync(filePath, cancellationToken);

        // Assert
        result.Should().Be(expectedUrl);
    }

    [Fact]
    public async Task When_GetPreSignedUrl_Called_Then_Calls_S3_With_Correct_Path()
    {
        // Arrange
        var filePath = "users/user-123/video/video.mp4";
        var expectedUrl = "https://s3.amazonaws.com/bucket/path";
        var cancellationToken = new CancellationToken();

        _s3BucketClientMock.GetPreSignedDownloadUrlAsync(filePath, cancellationToken)
            .Returns(expectedUrl);

        // Act
        await _sut.GetPreSignedDownloadUrlAsync(filePath, cancellationToken);

        // Assert
        await _s3BucketClientMock.Received(1).GetPreSignedDownloadUrlAsync(filePath, cancellationToken);
    }

    [Theory]
    [InlineData("users/user-1/video/video1.mp4", "https://s3.amazonaws.com/bucket/users/user-1/video/video1.mp4")]
    [InlineData("users/user-abc/video/movie.avi", "https://s3.amazonaws.com/bucket/users/user-abc/video/movie.avi")]
    [InlineData("users/user-xyz-123/edit/edit.zip", "https://s3.amazonaws.com/bucket/users/user-xyz-123/edit/edit.zip")]
    public async Task When_Different_Paths_Then_Returns_Correct_PreSignedUrls(string filePath, string expectedUrl)
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        _s3BucketClientMock.GetPreSignedDownloadUrlAsync(filePath, cancellationToken)
            .Returns(expectedUrl);

        // Act
        var result = await _sut.GetPreSignedDownloadUrlAsync(filePath, cancellationToken);

        // Assert
        result.Should().Be(expectedUrl);
    }

    [Fact]
    public async Task When_S3_GetPreSignedUrl_Throws_Exception_Then_Propagates_Exception()
    {
        // Arrange
        var filePath = "users/user-123/video/video.mp4";
        var expectedException = new InvalidOperationException("PreSigned URL generation failed");
        var cancellationToken = CancellationToken.None;

        _s3BucketClientMock.GetPreSignedDownloadUrlAsync(filePath, cancellationToken)
            .Returns(Task.FromException<string>(expectedException));

        // Act
        var act = async () => await _sut.GetPreSignedDownloadUrlAsync(filePath, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task When_Multiple_Paths_Then_Returns_Different_PreSignedUrls()
    {
        // Arrange
        var path1 = "users/user-1/video/video1.mp4";
        var path2 = "users/user-2/video/video2.mp4";
        var url1 = "https://s3.amazonaws.com/bucket/users/user-1/video/video1.mp4";
        var url2 = "https://s3.amazonaws.com/bucket/users/user-2/video/video2.mp4";
        var cancellationToken = CancellationToken.None;

        _s3BucketClientMock.GetPreSignedDownloadUrlAsync(path1, cancellationToken).Returns(url1);
        _s3BucketClientMock.GetPreSignedDownloadUrlAsync(path2, cancellationToken).Returns(url2);

        // Act
        var result1 = await _sut.GetPreSignedDownloadUrlAsync(path1, cancellationToken);
        var result2 = await _sut.GetPreSignedDownloadUrlAsync(path2, cancellationToken);

        // Assert
        result1.Should().Be(url1);
        result2.Should().Be(url2);
        result1.Should().NotBe(result2);
    }
}
