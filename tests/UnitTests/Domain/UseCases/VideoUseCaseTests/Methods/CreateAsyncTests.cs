using Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace UnitTests.Domain.UseCases.VideoUseCaseTests.Methods;

public class CreateAsyncTests : VideoUseCaseDependenciesMock
{
    [Fact]
    public async Task When_CreateAsync_Then_Returns_Id_From_Repository()
    {
        // Arrange
        var expectedId = "video-id-123";
        var video = new Video("userId", "path/to/video.mp4", "video.mp4", "video/mp4", 1024);

        _videoRepository.InsertOneAsync(video, Arg.Any<CancellationToken>()).Returns(expectedId);

        // Act
        var result = await _sut.CreateAsync(video, CancellationToken.None);

        // Assert
        result.Should().Be(expectedId);
        await _videoRepository.Received(1).InsertOneAsync(video, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task When_Repository_Throws_Exception_Then_Propagate_Exception()
    {
        // Arrange
        var video = new Video("userId", "path/to/video.mp4", "video.mp4", "video/mp4", 1024);
        var expectedException = new InvalidOperationException("Database error");

        _videoRepository
            .InsertOneAsync(video, Arg.Any<CancellationToken>())
            .Returns(Task.FromException<string>(expectedException));

        // Act
        var act = async () => await _sut.CreateAsync(video, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Database error");
    }
}
