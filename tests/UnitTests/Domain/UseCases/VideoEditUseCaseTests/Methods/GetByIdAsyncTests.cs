using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Entities.Exceptions;
using FluentAssertions;
using NSubstitute;

namespace UnitTests.Domain.UseCases.VideoEditUseCaseTests.Methods;

public class GetByIdAsyncTests : VideoEditUseCaseDependenciesMock
{
    [Fact]
    public async Task When_Not_Found_Then_Throw_EntityNotFoundException()
    {
        // Arrange
        var id = "not-found";
        _videoEditRepository.GetByIdAsync(id, Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns((VideoEdit?)null);

        // Act
        var act = async () => await _sut.GetByIdAsync(id, "userId", CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException<VideoEdit>>();
    }

    [Fact]
    public async Task When_Found_Then_Returns_VideoEdit()
    {
        // Arrange
        var id = "found-id";
        var expected = new VideoEdit(id, System.DateTime.UtcNow, "userId", "recipient", EditType.Frame, EditStatus.Created, "videoId", "editPath", new List<NotificationTarget>());

        _videoEditRepository.GetByIdAsync(id, Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(expected);

        // Act
        var result = await _sut.GetByIdAsync(id, "userId", CancellationToken.None);

        // Assert
        result.Should().BeSameAs(expected);
    }

    [Fact]
    public async Task When_Repository_Throws_Exception_Then_Propagate_Exception()
    {
        // Arrange
        var id = "error-id";
        var expectedException = new InvalidOperationException("Database connection failed");

        _videoEditRepository
            .GetByIdAsync(id, Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromException<VideoEdit?>(expectedException));

        // Act
        var act = async () => await _sut.GetByIdAsync(id, "userId", CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Database connection failed");
    }

    [Fact]
    public async Task When_CancellationToken_Is_Cancelled_Then_Throw_OperationCanceledException()
    {
        // Arrange
        var id = "cancel-id";
        var cts = new CancellationTokenSource();
        cts.Cancel();

        _videoEditRepository
            .GetByIdAsync(id, Arg.Any<string>(), cts.Token)
            .Returns(Task.FromException<VideoEdit?>(new OperationCanceledException()));

        // Act
        var act = async () => await _sut.GetByIdAsync(id, "userId", cts.Token);

        // Assert
        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task When_Different_UserIds_Then_Repository_Called_With_Correct_UserId()
    {
        // Arrange
        var id = "test-id";
        var userId = "user-123";
        var expected = new VideoEdit(id, System.DateTime.UtcNow, userId, "recipient", EditType.Frame, EditStatus.Created, "videoId", "editPath", new List<NotificationTarget>());

        _videoEditRepository
            .GetByIdAsync(id, userId, Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _sut.GetByIdAsync(id, userId, CancellationToken.None);

        // Assert
        result.Should().BeSameAs(expected);
        await _videoEditRepository.Received(1).GetByIdAsync(id, userId, Arg.Any<CancellationToken>());
    }

    [Theory]
    [InlineData("id1")]
    [InlineData("id-with-special-chars_@123")]
    [InlineData("UPPERCASE-ID")]
    public async Task When_Different_IDs_Then_Returns_Correct_VideoEdit(string testId)
    {
        // Arrange
        var expected = new VideoEdit(testId, System.DateTime.UtcNow, "userId", "recipient", EditType.Frame, EditStatus.Created, "videoId", "editPath", new List<NotificationTarget>());

        _videoEditRepository
            .GetByIdAsync(testId, Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(expected);

        // Act
        var result = await _sut.GetByIdAsync(testId, "userId", CancellationToken.None);

        // Assert
        result.Should().BeSameAs(expected);
        result.Id.Should().Be(testId);
    }

    [Fact]
    public async Task When_Repository_Throws_KeyNotFoundException_Then_Propagate_Exception()
    {
        // Arrange
        var id = "missing-id";
        var expectedException = new KeyNotFoundException($"VideoEdit with id '{id}' not found in database");

        _videoEditRepository
            .GetByIdAsync(id, Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromException<VideoEdit?>(expectedException));

        // Act
        var act = async () => await _sut.GetByIdAsync(id, "userId", CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<KeyNotFoundException>()
            .WithMessage($"*{id}*");
    }

    [Fact]
    public async Task When_Found_With_Different_Statuses_Then_Returns_VideoEdit()
    {
        // Arrange
        var id = "status-test-id";
        var videoEditProcessed = new VideoEdit(id, System.DateTime.UtcNow, "userId", "recipient", EditType.Frame, EditStatus.Processed, "videoId", "editPath", new List<NotificationTarget>());

        _videoEditRepository
            .GetByIdAsync(id, Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(videoEditProcessed);

        // Act
        var result = await _sut.GetByIdAsync(id, "userId", CancellationToken.None);

        // Assert
        result.Should().BeSameAs(videoEditProcessed);
        result.Status.Should().Be(EditStatus.Processed);
    }
}
