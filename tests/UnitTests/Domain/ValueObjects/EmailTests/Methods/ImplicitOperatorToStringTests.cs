using Domain.ValueObjects;
using FluentAssertions;

namespace UnitTests.Domain.ValueObjects.EmailTests.Methods;

public class ImplicitOperatorToStringTests
{
    [Theory]
    [InlineData("user@example.com")]
    [InlineData("test.email@domain.co.uk")]
    [InlineData("name+tag@company.com")]
    public void When_Email_Is_Implicitly_Converted_To_String_Then_Returns_Address(string emailAddress)
    {
        // Arrange
        var email = new Email(emailAddress);

        // Act
        string result = email;

        // Assert
        result.Should().Be(emailAddress);
    }

    [Fact]
    public void When_Email_Is_Implicitly_Converted_To_String_Then_Same_As_Property()
    {
        // Arrange
        var emailAddress = "test@example.com";
        var email = new Email(emailAddress);

        // Act
        string result = email;

        // Assert
        result.Should().Be(email.Adress);
    }
}
