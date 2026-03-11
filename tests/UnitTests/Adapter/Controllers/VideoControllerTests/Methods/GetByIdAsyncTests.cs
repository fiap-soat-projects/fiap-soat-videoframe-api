using Adapter.Presenters.DTOs;
using Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace UnitTests.Adapter.Controllers.VideoControllerTests.Methods;

public class GetByIdAsyncTests : VideoControllerDependenciesMock
{
    [Fact]
    public async Task When_Valid_Id_And_UserRequest_Then_Returns_GetVideoByIdPresenter()
    {
        // Arrange
        var id = "video-123";
        var userId = "user-123";
        var userRequest = new UserRequest(userId, "John Doe", "recipient@example.com");

        var video = new Video(
            id,
            DateTime.UtcNow,
            userId,
            "/videos/test.mp4",
            "test-video.mp4",
            "video/mp4",
            1024000);

        _videoUseCaseMock.GetByIdAsync(id, userId, Arg.Any<CancellationToken>())
            .Returns(video);

        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _sut.GetByIdAsync(id, userRequest, cancellationToken);

        // Assert
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData("id-1", "user-1")]
    [InlineData("id-abc-xyz", "user-abc")]
    [InlineData("123456", "987654")]
    public async Task When_Different_Id_And_UserId_Then_Calls_UseCase_With_Correct_Parameters(string id, string userId)
    {
        // Arrange
        var userRequest = new UserRequest(userId, "John Doe", "recipient@example.com");
        var cancellationToken = new CancellationToken();

        var video = new Video(id, DateTime.UtcNow, userId, "/path", "name", "video/mp4", 1024);
        _videoUseCaseMock.GetByIdAsync(id, userId, cancellationToken).Returns(video);

        // Act
        await _sut.GetByIdAsync(id, userRequest, cancellationToken);

        // Assert
        await _videoUseCaseMock.Received(1).GetByIdAsync(id, userId, cancellationToken);
    }

    [Fact]
    public async Task When_UseCase_Throws_Exception_Then_Propagates_Exception()
    {
        // Arrange
        var id = "video-123";
        var userId = "user-123";
        var userRequest = new UserRequest(userId, "John Doe", "recipient@example.com");
        var expectedException = new KeyNotFoundException("Video not found");

        _videoUseCaseMock.GetByIdAsync(id, userId, Arg.Any<CancellationToken>())
            .Returns(Task.FromException<Video>(expectedException));

        var cancellationToken = CancellationToken.None;

        // Act
        var act = async () => await _sut.GetByIdAsync(id, userRequest, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}
