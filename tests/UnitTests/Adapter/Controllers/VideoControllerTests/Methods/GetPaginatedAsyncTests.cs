using Adapter.Presenters.DTOs;
using Domain.Entities;
using Domain.Entities.Page;
using FluentAssertions;
using NSubstitute;

namespace UnitTests.Adapter.Controllers.VideoControllerTests.Methods;

public class GetPaginatedAsyncTests : VideoControllerDependenciesMock
{
    [Fact]
    public async Task When_Valid_UserRequest_And_PaginationRequest_Then_Returns_GetPaginatedVideosPresenter()
    {
        // Arrange
        var userId = "user-123";
        var userRequest = new UserRequest(userId, "John Doe", "recipient@example.com");
        var page = 1;
        var size = 10;
        var paginationRequest = new PaginationRequest(page, size);

        var videos = new List<Video>
        {
            new Video(userId, "/path1", "video1.mp4", "video/mp4", 1024000),
            new Video(userId, "/path2", "video2.mp4", "video/mp4", 2048000)
        };

        var pagination = new Pagination<Video>
        {
            Page = page,
            Size = size,
            TotalCount = 2,
            TotalPages = 1,
            Items = videos
        };

        _videoUseCaseMock.GetPaginatedAsync(userId, page, size, Arg.Any<CancellationToken>())
            .Returns(pagination);

        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _sut.GetPaginatedAsync(userRequest, paginationRequest, cancellationToken);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task When_Different_Page_Numbers_Then_Calls_UseCase_With_Correct_Page()
    {
        // Arrange
        var userId = "user-123";
        var userRequest = new UserRequest(userId, "John Doe", "recipient@example.com");
        var page = 5;
        var size = 20;
        var paginationRequest = new PaginationRequest(page, size);
        var cancellationToken = new CancellationToken();

        var pagination = new Pagination<Video>
        {
            Page = page,
            Size = size,
            TotalCount = 0,
            TotalPages = 0,
            Items = new List<Video>()
        };
        _videoUseCaseMock.GetPaginatedAsync(userId, page, size, cancellationToken).Returns(pagination);

        // Act
        await _sut.GetPaginatedAsync(userRequest, paginationRequest, cancellationToken);

        // Assert
        await _videoUseCaseMock.Received(1).GetPaginatedAsync(userId, page, size, cancellationToken);
    }

    [Fact]
    public async Task When_UseCase_Throws_Exception_Then_Propagates_Exception()
    {
        // Arrange
        var userId = "user-123";
        var userRequest = new UserRequest(userId, "John Doe", "recipient@example.com");
        var paginationRequest = new PaginationRequest(1, 10);
        var expectedException = new InvalidOperationException("Database error");

        _videoUseCaseMock.GetPaginatedAsync(userId, 1, 10, Arg.Any<CancellationToken>())
            .Returns(Task.FromException<Pagination<Video>>(expectedException));

        var cancellationToken = CancellationToken.None;

        // Act
        var act = async () => await _sut.GetPaginatedAsync(userRequest, paginationRequest, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
