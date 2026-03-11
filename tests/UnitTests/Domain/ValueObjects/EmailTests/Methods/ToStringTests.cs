using Domain.ValueObjects;
using FluentAssertions;

namespace UnitTests.Domain.ValueObjects.EmailTests.Methods;

public class ToStringTests : EmailDependenciesMock
{
    [Theory]
    [InlineData("user@example.com")]
    [InlineData("test.email@domain.co.uk")]
    [InlineData("name+tag@company.com")]
    public void When_ToString_Is_Called_Then_Returns_Address(string emailAddress)
    {
        // Arrange
        var email = new Email(emailAddress);

        // Act
        var result = email.ToString();

        // Assert
        result.Should().Be(emailAddress);
    }

    [Fact]
    public void When_ToString_Is_Called_Then_Same_As_Property()
    {
        // Arrange
        var emailAddress = "test@example.com";
        var email = new Email(emailAddress);

        // Act
        var result = email.ToString();

        // Assert
        result.Should().Be(email.Adress);
    }

    [Fact]
    public void When_ToString_Is_Called_On_Sut_Then_Returns_Valid_Address()
    {
        // Act
        var result = _sut.ToString();

        // Assert
        result.Should().Be("test@example.com");
    }
}
