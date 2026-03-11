using Domain.Entities;
using Domain.Entities.Page;
using FluentAssertions;
using NSubstitute;

namespace UnitTests.Domain.UseCases.VideoUseCaseTests.Methods;

public class GetPaginatedAsyncTests : VideoUseCaseDependenciesMock
{
    [Fact]
    public async Task When_Called_Then_Returns_Pagination_From_Repository()
    {
        // Arrange
        var userId = "user-123";
        var page = 1;
        var size = 10;

        var paginationMock = Substitute.For<Pagination<Video>>();

        _videoRepository.GetPaginatedAsync(userId, page, size, Arg.Any<CancellationToken>()).Returns(paginationMock);

        // Act
        var result = await _sut.GetPaginatedAsync(userId, page, size, CancellationToken.None);

        // Assert
        result.Should().BeSameAs(paginationMock);
        await _videoRepository.Received(1).GetPaginatedAsync(userId, page, size, Arg.Any<CancellationToken>());
    }

    [Theory]
    [InlineData(1, 10)]
    [InlineData(2, 20)]
    [InlineData(5, 50)]
    public async Task When_Different_Pages_And_Sizes_Then_Repository_Called_With_Correct_Parameters(int page, int size)
    {
        // Arrange
        var userId = "user-123";
        var paginationMock = Substitute.For<Pagination<Video>>();

        _videoRepository.GetPaginatedAsync(userId, page, size, Arg.Any<CancellationToken>()).Returns(paginationMock);

        // Act
        var result = await _sut.GetPaginatedAsync(userId, page, size, CancellationToken.None);

        // Assert
        result.Should().BeSameAs(paginationMock);
        await _videoRepository.Received(1).GetPaginatedAsync(userId, page, size, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task When_Repository_Throws_Exception_Then_Propagate_Exception()
    {
        // Arrange
        var userId = "user-123";
        var page = 1;
        var size = 10;
        var expectedException = new InvalidOperationException("Database error");

        _videoRepository
            .GetPaginatedAsync(userId, page, size, Arg.Any<CancellationToken>())
            .Returns(Task.FromException<Pagination<Video>>(expectedException));

        // Act
        var act = async () => await _sut.GetPaginatedAsync(userId, page, size, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Database error");
    }
}
