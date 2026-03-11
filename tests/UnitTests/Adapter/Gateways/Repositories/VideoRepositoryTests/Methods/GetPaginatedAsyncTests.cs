using Domain.Entities;
using Domain.Entities.Page;
using FluentAssertions;
using Infrastructure.Entities;
using Infrastructure.Entities.Page;
using NSubstitute;

namespace UnitTests.Adapter.Gateways.Repositories.VideoRepositoryTests.Methods;

public class GetPaginatedAsyncTests : VideoRepositoryDependenciesMock
{
    [Fact]
    public async Task When_Valid_Parameters_Then_Returns_Paginated_Videos()
    {
        // Arrange
        var userId = "user-123";
        var page = 1;
        var size = 10;
        var cancellationToken = CancellationToken.None;

        var mongoDbEntity = new VideoMongoDb(new Video(
            "video-1",
            DateTime.UtcNow,
            "user-123",
            "/path/to/video.mp4",
            "video.mp4",
            "video/mp4",
            1024000
        ))
        {
            Id = "video-1"
        };

        var pagedResult = new PagedResult<VideoMongoDb>
        {
            Page = 1,
            Size = 10,
            TotalCount = 1,
            TotalPages = 1,
            Items = [mongoDbEntity]
        };

        _videoMongoDbRepositoryMock.GetAllAsync(userId, page, size, cancellationToken)
            .Returns(pagedResult);

        // Act
        var result = await _sut.GetPaginatedAsync(userId, page, size, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Pagination<Video>>();
        result.Page.Should().Be(1);
        result.Size.Should().Be(10);
        result.TotalCount.Should().Be(1);
        result.TotalPages.Should().Be(1);
        result.Items.Should().HaveCount(1);
    }

    [Fact]
    public async Task When_GetPaginatedAsync_Called_Then_Calls_Repository_GetAllAsync()
    {
        // Arrange
        var userId = "user-123";
        var page = 1;
        var size = 20;
        var cancellationToken = new CancellationToken();

        var pagedResult = new PagedResult<VideoMongoDb>
        {
            Items = []
        };

        _videoMongoDbRepositoryMock.GetAllAsync(userId, page, size, cancellationToken)
            .Returns(pagedResult);

        // Act
        await _sut.GetPaginatedAsync(userId, page, size, cancellationToken);

        // Assert
        await _videoMongoDbRepositoryMock.Received(1).GetAllAsync(userId, page, size, cancellationToken);
    }

    [Theory]
    [InlineData("user-1", 1, 10)]
    [InlineData("user-abc", 2, 20)]
    [InlineData("user-xyz", 3, 5)]
    public async Task When_Different_Parameters_Then_Returns_Correct_Pagination(string userId, int page, int size)
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        var pagedResult = new PagedResult<VideoMongoDb>
        {
            Page = page,
            Size = size,
            TotalCount = 100,
            TotalPages = 10,
            Items = []
        };

        _videoMongoDbRepositoryMock.GetAllAsync(userId, page, size, cancellationToken)
            .Returns(pagedResult);

        // Act
        var result = await _sut.GetPaginatedAsync(userId, page, size, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Page.Should().Be(page);
        result.Size.Should().Be(size);
        await _videoMongoDbRepositoryMock.Received(1).GetAllAsync(userId, page, size, cancellationToken);
    }

    [Fact]
    public async Task When_Empty_Result_Then_Returns_Empty_Pagination()
    {
        // Arrange
        var userId = "user-123";
        var page = 1;
        var size = 10;
        var cancellationToken = CancellationToken.None;

        var pagedResult = new PagedResult<VideoMongoDb>
        {
            Page = 1,
            Size = 10,
            TotalCount = 0,
            TotalPages = 0,
            Items = []
        };

        _videoMongoDbRepositoryMock.GetAllAsync(userId, page, size, cancellationToken)
            .Returns(pagedResult);

        // Act
        var result = await _sut.GetPaginatedAsync(userId, page, size, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
    }

    [Fact]
    public async Task When_Multiple_Items_Then_Returns_All_Converted_Items()
    {
        // Arrange
        var userId = "user-123";
        var page = 1;
        var size = 10;
        var cancellationToken = CancellationToken.None;

        var video1 = new Video(
            "video-1",
            DateTime.UtcNow,
            "user-123",
            "/path/to/video1.mp4",
            "video1.mp4",
            "video/mp4",
            1024000
        );

        var video2 = new Video(
            "video-2",
            DateTime.UtcNow,
            "user-123",
            "/path/to/video2.mp4",
            "video2.mp4",
            "video/mp4",
            2048000
        );

        var pagedResult = new PagedResult<VideoMongoDb>
        {
            Page = 1,
            Size = 10,
            TotalCount = 2,
            TotalPages = 1,
            Items = [
                new VideoMongoDb(video1) { Id = "video-1" },
                new VideoMongoDb(video2) { Id = "video-2" }
            ]
        };

        _videoMongoDbRepositoryMock.GetAllAsync(userId, page, size, cancellationToken)
            .Returns(pagedResult);

        // Act
        var result = await _sut.GetPaginatedAsync(userId, page, size, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
    }
}
