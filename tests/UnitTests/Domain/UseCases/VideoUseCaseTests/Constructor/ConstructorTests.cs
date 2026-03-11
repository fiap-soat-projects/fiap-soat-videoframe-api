using FluentAssertions;

namespace UnitTests.Domain.UseCases.VideoUseCaseTests.Constructor;

public class ConstructorTests : VideoUseCaseDependenciesMock
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
