using Domain.Gateways.Clients.DTOs;
using FluentAssertions;
using NSubstitute;

namespace UnitTests.Domain.UseCases.VideoUseCaseTests.Methods;

public class UploadAsyncTests : VideoUseCaseDependenciesMock
{
    [Fact]
    public async Task When_UploadAsync_Then_Returns_Path_From_BucketClient()
    {
        // Arrange
        var fileUpload = new FileUpload("user-123", "video.mp4", 1024, "video/mp4", new MemoryStream());
        var expectedPath = "videos/video-123.mp4";

        _bucketClient.UploadFileAsync(fileUpload, Arg.Any<CancellationToken>()).Returns(expectedPath);

        // Act
        var result = await _sut.UploadAsync(fileUpload, CancellationToken.None);

        // Assert
        result.Should().Be(expectedPath);
        await _bucketClient.Received(1).UploadFileAsync(fileUpload, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task When_BucketClient_Throws_Exception_Then_Propagate_Exception()
    {
        // Arrange
        var fileUpload = new FileUpload("user-123", "video.mp4", 1024, "video/mp4", new MemoryStream());
        var expectedException = new IOException("S3 upload failed");

        _bucketClient
            .UploadFileAsync(fileUpload, Arg.Any<CancellationToken>())
            .Returns(Task.FromException<string>(expectedException));

        // Act
        var act = async () => await _sut.UploadAsync(fileUpload, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<IOException>()
            .WithMessage("S3 upload failed");
    }

    [Theory]
    [InlineData("video1.mp4")]
    [InlineData("large-video-file_@123.mp4")]
    public async Task When_Different_Filenames_Then_Returns_Correct_Path(string filename)
    {
        // Arrange
        var fileUpload = new FileUpload("user-123", filename, 1024, "video/mp4", new MemoryStream());
        var expectedPath = $"videos/{filename}";

        _bucketClient.UploadFileAsync(fileUpload, Arg.Any<CancellationToken>()).Returns(expectedPath);

        // Act
        var result = await _sut.UploadAsync(fileUpload, CancellationToken.None);

        // Assert
        result.Should().Be(expectedPath);
        result.Should().Contain(filename);
    }
}
