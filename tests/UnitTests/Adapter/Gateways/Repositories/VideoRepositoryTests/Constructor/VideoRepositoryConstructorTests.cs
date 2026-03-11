using Adapter.Gateways.Repositories;
using FluentAssertions;
using Infrastructure.Repositories.Interfaces;
using NSubstitute;

namespace UnitTests.Adapter.Gateways.Repositories.VideoRepositoryTests.Constructor;

public class VideoRepositoryConstructorTests
{
    [Fact]
    public void When_Valid_IVideoMongoDbRepository_Is_Provided_Then_Construction_Succeeds()
    {
        // Arrange
        var videoMongoDbRepositoryMock = Substitute.For<IVideoMongoDbRepository>();

        // Act
        var repository = new VideoRepository(videoMongoDbRepositoryMock);

        // Assert
        repository.Should().NotBeNull();
    }
}
