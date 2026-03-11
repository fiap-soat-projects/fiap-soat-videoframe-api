using FluentAssertions;

namespace UnitTests.Domain.UseCases.VideoEditUseCaseTests.Constructor;

public class ConstructorTests : UnitTests.Domain.UseCases.VideoEditUseCaseTests.VideoEditUseCaseDependenciesMock
{
    [Fact]
    public void When_Dependencies_Are_Provided_Then_Construct_Succeeds()
    {
        // Arrange is done by base

        // Act
        var sut = _sut;

        // Assert
        sut.Should().NotBeNull();
    }
}
