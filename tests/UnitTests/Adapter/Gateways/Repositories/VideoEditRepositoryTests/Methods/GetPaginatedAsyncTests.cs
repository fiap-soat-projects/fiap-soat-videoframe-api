using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Entities.Page;
using FluentAssertions;
using Infrastructure.Entities;
using Infrastructure.Entities.Page;
using NSubstitute;

namespace UnitTests.Adapter.Gateways.Repositories.VideoEditRepositoryTests.Methods;

public class GetPaginatedAsyncTests : VideoEditRepositoryDependenciesMock
{
    [Fact]
    public async Task When_Valid_Parameters_Then_Returns_Paginated_VideoEdits()
    {
        // Arrange
        var userId = "user-123";
        var skip = 0;
        var limit = 10;
        var cancellationToken = CancellationToken.None;

        var mongoDbEntity = new VideoEditMongoDb(new VideoEdit(
            "edit-1",
            DateTime.UtcNow,
            "user-123",
            "recipient@example.com",
            EditType.Frame,
            EditStatus.Created,
            "video-1",
            "/path/to/edit",
            []
        ))
        {
            Id = "edit-1"
        };

        var pagedResult = new PagedResult<VideoEditMongoDb>
        {
            Page = 1,
            Size = 10,
            TotalCount = 1,
            TotalPages = 1,
            Items = [mongoDbEntity]
        };

        _videoEditMongoDbRepositoryMock.GetAllAsync(userId, skip, limit, cancellationToken)
            .Returns(pagedResult);

        // Act
        var result = await _sut.GetPaginatedAsync(userId, skip, limit, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Pagination<VideoEdit>>();
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
        var skip = 5;
        var limit = 20;
        var cancellationToken = new CancellationToken();

        var pagedResult = new PagedResult<VideoEditMongoDb>
        {
            Items = []
        };

        _videoEditMongoDbRepositoryMock.GetAllAsync(userId, skip, limit, cancellationToken)
            .Returns(pagedResult);

        // Act
        await _sut.GetPaginatedAsync(userId, skip, limit, cancellationToken);

        // Assert
        await _videoEditMongoDbRepositoryMock.Received(1).GetAllAsync(userId, skip, limit, cancellationToken);
    }

    [Theory]
    [InlineData("user-1", 0, 10)]
    [InlineData("user-abc", 10, 20)]
    [InlineData("user-xyz", 20, 5)]
    public async Task When_Different_Parameters_Then_Returns_Correct_Pagination(string userId, int skip, int limit)
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        var pagedResult = new PagedResult<VideoEditMongoDb>
        {
            Page = 1,
            Size = limit,
            TotalCount = 100,
            TotalPages = 10,
            Items = []
        };

        _videoEditMongoDbRepositoryMock.GetAllAsync(userId, skip, limit, cancellationToken)
            .Returns(pagedResult);

        // Act
        var result = await _sut.GetPaginatedAsync(userId, skip, limit, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Size.Should().Be(limit);
        await _videoEditMongoDbRepositoryMock.Received(1).GetAllAsync(userId, skip, limit, cancellationToken);
    }

    [Fact]
    public async Task When_Empty_Result_Then_Returns_Empty_Pagination()
    {
        // Arrange
        var userId = "user-123";
        var skip = 0;
        var limit = 10;
        var cancellationToken = CancellationToken.None;

        var pagedResult = new PagedResult<VideoEditMongoDb>
        {
            Page = 1,
            Size = 10,
            TotalCount = 0,
            TotalPages = 0,
            Items = []
        };

        _videoEditMongoDbRepositoryMock.GetAllAsync(userId, skip, limit, cancellationToken)
            .Returns(pagedResult);

        // Act
        var result = await _sut.GetPaginatedAsync(userId, skip, limit, cancellationToken);

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
        var skip = 0;
        var limit = 10;
        var cancellationToken = CancellationToken.None;

        var videoEdit1 = new VideoEdit(
            "edit-1",
            DateTime.UtcNow,
            "user-123",
            "recipient1@example.com",
            EditType.Frame,
            EditStatus.Created,
            "video-1",
            "/path/to/edit1",
            []
        );

        var videoEdit2 = new VideoEdit(
            "edit-2",
            DateTime.UtcNow,
            "user-123",
            "recipient2@example.com",
            EditType.Frame,
            EditStatus.Processing,
            "video-2",
            "/path/to/edit2",
            []
        );

        var pagedResult = new PagedResult<VideoEditMongoDb>
        {
            Page = 1,
            Size = 10,
            TotalCount = 2,
            TotalPages = 1,
            Items = [
                new VideoEditMongoDb(videoEdit1) { Id = "edit-1" },
                new VideoEditMongoDb(videoEdit2) { Id = "edit-2" }
            ]
        };

        _videoEditMongoDbRepositoryMock.GetAllAsync(userId, skip, limit, cancellationToken)
            .Returns(pagedResult);

        // Act
        var result = await _sut.GetPaginatedAsync(userId, skip, limit, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
    }
}
