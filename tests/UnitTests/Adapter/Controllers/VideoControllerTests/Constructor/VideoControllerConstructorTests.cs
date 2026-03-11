using Adapter.Controllers;
using Domain.UseCases.Interfaces;
using FluentAssertions;
using NSubstitute;

namespace UnitTests.Adapter.Controllers.VideoControllerTests.Constructor;

public class VideoControllerConstructorTests
{
    [Fact]
    public void When_Valid_IVideoUseCase_Is_Provided_Then_Construction_Succeeds()
    {
        // Arrange
        var videoUseCaseMock = Substitute.For<IVideoUseCase>();

        // Act
        var controller = new VideoController(videoUseCaseMock);

        // Assert
        controller.Should().NotBeNull();
    }
}
