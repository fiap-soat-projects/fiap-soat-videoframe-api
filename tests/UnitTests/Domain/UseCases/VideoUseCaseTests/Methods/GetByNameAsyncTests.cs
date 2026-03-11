using Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace UnitTests.Domain.UseCases.VideoUseCaseTests.Methods;

public class GetByNameAsyncTests : VideoUseCaseDependenciesMock
{
    [Fact]
    public async Task When_Found_Then_Returns_Video()
    {
        // Arrange
        var name = "video.mp4";
        var userId = "user-123";
        var expected = new Video("video-123", DateTime.UtcNow, userId, "path/to/video.mp4", name, "video/mp4", 1024);

        _videoRepository.GetByNameAsync(name, userId, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await _sut.GetByNameAsync(name, userId, CancellationToken.None);

        // Assert
        result.Should().BeSameAs(expected);
        result!.Name.Should().Be(name);
    }

    [Fact]
    public async Task When_Not_Found_Then_Returns_Null()
    {
        // Arrange
        var name = "not-found.mp4";
        var userId = "user-123";

        _videoRepository.GetByNameAsync(name, userId, Arg.Any<CancellationToken>()).Returns((Video?)null);

        // Act
        var result = await _sut.GetByNameAsync(name, userId, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task When_Repository_Throws_Exception_Then_Propagate_Exception()
    {
        // Arrange
        var name = "video.mp4";
        var userId = "user-123";
        var expectedException = new InvalidOperationException("Database error");

        _videoRepository
            .GetByNameAsync(name, userId, Arg.Any<CancellationToken>())
            .Returns(Task.FromException<Video?>(expectedException));

        // Act
        var act = async () => await _sut.GetByNameAsync(name, userId, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Database error");
    }

    [Theory]
    [InlineData("video1.mp4")]
    [InlineData("video-special-chars_@123.mp4")]
    public async Task When_Different_Names_Then_Returns_Correct_Video(string testName)
    {
        // Arrange
        var userId = "user-123";
        var expected = new Video("video-123", DateTime.UtcNow, userId, "path/to/video.mp4", testName, "video/mp4", 1024);

        _videoRepository.GetByNameAsync(testName, userId, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await _sut.GetByNameAsync(testName, userId, CancellationToken.None);

        // Assert
        result.Should().BeSameAs(expected);
        result!.Name.Should().Be(testName);
    }
}
