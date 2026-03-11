using FluentAssertions;
using NSubstitute;

namespace UnitTests.Domain.UseCases.VideoEditUseCaseTests.Methods;

public class DownloadAsyncTests : VideoEditUseCaseDependenciesMock
{
    [Fact]
    public async Task When_DownloadAsync_Then_Returns_Stream_From_BucketClient()
    {
        // Arrange
        var path = "some/path";
        var expectedStream = new MemoryStream();

        _bucketClient.DownloadFileAsync(path, Arg.Any<CancellationToken>()).Returns(expectedStream);

        // Act
        var result = await _sut.DownloadAsync(path, CancellationToken.None);

        // Assert
        result.Should().BeSameAs(expectedStream);
        await _bucketClient.Received(1).DownloadFileAsync(path, Arg.Any<CancellationToken>());
    }
}
