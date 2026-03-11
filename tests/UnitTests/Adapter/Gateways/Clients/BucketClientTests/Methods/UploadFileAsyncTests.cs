using Domain.Gateways.Clients.DTOs;
using FluentAssertions;
using NSubstitute;

namespace UnitTests.Adapter.Gateways.Clients.BucketClientTests.Methods;

public class UploadFileAsyncTests : BucketClientDependenciesMock
{
    [Fact]
    public async Task When_Valid_FileUpload_Then_Returns_Correct_Path()
    {
        // Arrange
        var userId = "user-123";
        var fileName = "video.mp4";
        var fileType = "video";
        var fileLength = 1024000L;
        var fileStream = new MemoryStream(new byte[] { 1, 2, 3 });

        var fileUpload = new FileUpload(userId, fileName, fileLength, fileType, fileStream);
        var cancellationToken = CancellationToken.None;

        _s3BucketClientMock.UploadAsync(Arg.Any<string>(), fileStream, cancellationToken)
            .Returns(Task.CompletedTask);

        var expectedPath = $"users/{userId}/{fileType}/{fileName}";

        // Act
        var result = await _sut.UploadFileAsync(fileUpload, cancellationToken);

        // Assert
        result.Should().Be(expectedPath);
    }

    [Fact]
    public async Task When_File_Is_Uploaded_Then_Calls_S3_With_Correct_Path()
    {
        // Arrange
        var userId = "user-123";
        var fileName = "video.mp4";
        var fileType = "video";
        var fileStream = new MemoryStream();
        var fileUpload = new FileUpload(userId, fileName, 1024000, fileType, fileStream);
        var cancellationToken = new CancellationToken();

        _s3BucketClientMock.UploadAsync(Arg.Any<string>(), fileStream, cancellationToken)
            .Returns(Task.CompletedTask);

        var expectedPath = $"users/{userId}/{fileType}/{fileName}";

        // Act
        await _sut.UploadFileAsync(fileUpload, cancellationToken);

        // Assert
        await _s3BucketClientMock.Received(1).UploadAsync(expectedPath, fileStream, cancellationToken);
    }

    [Theory]
    [InlineData("user-1", "video1.mp4", "video")]
    [InlineData("user-abc", "file.avi", "video")]
    [InlineData("user-xyz-123", "edit.zip", "edit")]
    public async Task When_Different_Users_And_Files_Then_Returns_Correct_Paths(
        string userId, string fileName, string fileType)
    {
        // Arrange
        var fileStream = new MemoryStream();
        var fileUpload = new FileUpload(userId, fileName, 1024000, fileType, fileStream);
        var cancellationToken = CancellationToken.None;

        _s3BucketClientMock.UploadAsync(Arg.Any<string>(), fileStream, cancellationToken)
            .Returns(Task.CompletedTask);

        var expectedPath = $"users/{userId}/{fileType}/{fileName}";

        // Act
        var result = await _sut.UploadFileAsync(fileUpload, cancellationToken);

        // Assert
        result.Should().Be(expectedPath);
    }

    [Fact]
    public async Task When_S3_Upload_Throws_Exception_Then_Propagates_Exception()
    {
        // Arrange
        var fileUpload = new FileUpload("user-123", "video.mp4", 1024000, "video", new MemoryStream());
        var expectedException = new IOException("Upload failed");
        var cancellationToken = CancellationToken.None;

        _s3BucketClientMock.UploadAsync(Arg.Any<string>(), Arg.Any<Stream>(), cancellationToken)
            .Returns(Task.FromException(expectedException));

        // Act
        var act = async () => await _sut.UploadFileAsync(fileUpload, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<IOException>();
    }

    [Fact]
    public async Task When_Multiple_Files_Uploaded_Then_Generates_Different_Paths()
    {
        // Arrange
        var file1 = new FileUpload("user-1", "video1.mp4", 1024000, "video", new MemoryStream());
        var file2 = new FileUpload("user-2", "video2.mp4", 2048000, "video", new MemoryStream());
        var cancellationToken = CancellationToken.None;

        _s3BucketClientMock.UploadAsync(Arg.Any<string>(), Arg.Any<Stream>(), cancellationToken)
            .Returns(Task.CompletedTask);

        var expectedPath1 = "users/user-1/video/video1.mp4";
        var expectedPath2 = "users/user-2/video/video2.mp4";

        // Act
        var result1 = await _sut.UploadFileAsync(file1, cancellationToken);
        var result2 = await _sut.UploadFileAsync(file2, cancellationToken);

        // Assert
        result1.Should().Be(expectedPath1);
        result2.Should().Be(expectedPath2);
    }
}
