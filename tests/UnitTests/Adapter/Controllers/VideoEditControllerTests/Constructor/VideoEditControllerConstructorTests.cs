using Adapter.Controllers;
using Domain.UseCases.Interfaces;
using FluentAssertions;
using NSubstitute;

namespace UnitTests.Adapter.Controllers.VideoEditControllerTests.Constructor;

public class VideoEditControllerConstructorTests
{
    [Fact]
    public void When_Valid_Dependencies_Are_Provided_Then_Construction_Succeeds()
    {
        // Arrange
        var videoEditUseCaseMock = Substitute.For<IVideoEditUseCase>();
        var videoUseCaseMock = Substitute.For<IVideoUseCase>();

        // Act
        var controller = new VideoEditController(videoEditUseCaseMock, videoUseCaseMock);

        // Assert
        controller.Should().NotBeNull();
    }
}
