using FluentAssertions;
using Domain.Entities;
using NSubstitute;
using Domain.Entities.Page;

namespace UnitTests.Domain.UseCases.VideoEditUseCaseTests.Methods;

public class GetPaginatedAsyncTests : VideoEditUseCaseDependenciesMock
{
    [Fact]
    public async Task When_Called_Then_Returns_Pagination_From_Repository()
    {
        // Arrange
        var userId = "user";
        var page = 1;
        var size = 10;

        var paginationMock = Substitute.For<Pagination<VideoEdit>>();

        _videoEditRepository.GetPaginatedAsync(userId, page, size, Arg.Any<CancellationToken>()).Returns(paginationMock);

        // Act
        var result = await _sut.GetPaginatedAsync(userId, page, size, CancellationToken.None);

        // Assert
        result.Should().BeSameAs(paginationMock);
    }
}
