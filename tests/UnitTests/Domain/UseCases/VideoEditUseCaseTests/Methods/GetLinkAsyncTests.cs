using Domain.Entities;
using Domain.Entities.Enums;
using FluentAssertions;
using NSubstitute;

namespace UnitTests.Domain.UseCases.VideoEditUseCaseTests.Methods;

public class GetLinkAsyncTests : VideoEditUseCaseDependenciesMock
{
    [Fact]
    public async Task When_Status_Is_Not_Processed_Then_Throw_Exception()
    {
        // Arrange
        var id = "id1";
        var videoEdit = new VideoEdit(id, DateTime.UtcNow, "userId", "recipient", EditType.Frame, EditStatus.Created, "videoId", default!, new List<NotificationTarget>());

        _videoEditRepository.GetByIdAsync(id, Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(videoEdit);

        // Act
        var act = async () => await _sut.GetLinkAsync(id, "userId", CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("This edit is not processed");
    }

    [Fact]
    public async Task When_Processed_Then_Returns_PresignedUrl()
    {
        // Arrange
        var id = "id2";
        var editPath = "path/to/edit";
        var expectedUrl = "https://presigned.url";

        var videoEdit = new VideoEdit(id, DateTime.UtcNow, "userId", "recipient", EditType.Frame, EditStatus.Processed, "videoId", editPath, new List<NotificationTarget>());

        _videoEditRepository.GetByIdAsync(id, Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(videoEdit);

        _bucketClient.GetPreSignedDownloadUrlAsync(editPath, Arg.Any<CancellationToken>()).Returns(expectedUrl);

        // Act
        var result = await _sut.GetLinkAsync(id, "userId", CancellationToken.None);

        // Assert
        result.Should().Be(expectedUrl);
        await _bucketClient.Received(1).GetPreSignedDownloadUrlAsync(editPath, Arg.Any<CancellationToken>());
    }
}
