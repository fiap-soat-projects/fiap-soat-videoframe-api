using Domain.Entities;
using Domain.Entities.Exceptions;
using FluentAssertions;
using NSubstitute;

namespace UnitTests.Domain.UseCases.VideoUseCaseTests.Methods;

public class GetByIdAsyncTests : VideoUseCaseDependenciesMock
{
    [Fact]
    public async Task When_Found_Then_Returns_Video()
    {
        // Arrange
        var id = "video-123";
        var userId = "user-123";
        var expected = new Video(id, DateTime.UtcNow, userId, "path/to/video.mp4", "video.mp4", "video/mp4", 1024);

        _videoRepository.GetByIdAsync(id, userId, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await _sut.GetByIdAsync(id, userId, CancellationToken.None);

        // Assert
        result.Should().BeSameAs(expected);
    }

    [Fact]
    public async Task When_Not_Found_Then_Throw_EntityNotFoundException()
    {
        // Arrange
        var id = "not-found";
        var userId = "user-123";

        _videoRepository.GetByIdAsync(id, userId, Arg.Any<CancellationToken>()).Returns((Video?)null);

        // Act
        var act = async () => await _sut.GetByIdAsync(id, userId, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<Video>>();
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
        var act = async () => await _sut.GetByIdAsync(id, userId, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Database error");
    }

    [Theory]
    [InlineData("id1", "user1")]
    [InlineData("id-special-chars_@123", "user-special-chars_@123")]
    public async Task When_Different_Ids_And_UserIds_Then_Returns_Correct_Video(string testId, string testUserId)
    {
        // Arrange
        var expected = new Video(testId, DateTime.UtcNow, testUserId, "path/to/video.mp4", "video.mp4", "video/mp4", 1024);

        _videoRepository.GetByIdAsync(testId, testUserId, Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await _sut.GetByIdAsync(testId, testUserId, CancellationToken.None);

        // Assert
        result.Should().BeSameAs(expected);
        result.Id.Should().Be(testId);
        result.UserId.Should().Be(testUserId);
    }
}
