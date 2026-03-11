using Adapter.Gateways.Repositories;
using FluentAssertions;
using Infrastructure.Repositories.Interfaces;
using NSubstitute;

namespace UnitTests.Adapter.Gateways.Repositories.VideoEditRepositoryTests.Constructor;

public class VideoEditRepositoryConstructorTests
{
    [Fact]
    public void When_Valid_IVideoEditMongoDbRepository_Is_Provided_Then_Construction_Succeeds()
    {
        // Arrange
        var videoEditMongoDbRepositoryMock = Substitute.For<IVideoEditMongoDbRepository>();

        // Act
        var repository = new VideoEditRepository(videoEditMongoDbRepositoryMock);

        // Assert
        repository.Should().NotBeNull();
    }
}
