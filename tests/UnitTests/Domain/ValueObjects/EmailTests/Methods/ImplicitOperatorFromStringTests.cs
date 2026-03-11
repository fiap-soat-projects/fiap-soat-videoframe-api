using Domain.ValueObjects;
using Domain.ValueObjects.Exceptions;
using FluentAssertions;

namespace UnitTests.Domain.ValueObjects.EmailTests.Methods;

public class ImplicitOperatorFromStringTests
{
    [Theory]
    [InlineData("user@example.com")]
    [InlineData("test.email@domain.co.uk")]
    [InlineData("name+tag@company.com")]
    public void When_Valid_String_Is_Implicitly_Converted_To_Email_Then_Returns_Email(string emailString)
    {
        // Arrange & Act
        Email email = emailString;

        // Assert
        email.Adress.Should().Be(emailString);
    }

    [Fact]
    public void When_Null_String_Is_Implicitly_Converted_To_Email_Then_Throw_ArgumentException()
    {
        // Arrange
        string? nullEmail = null;

        // Act
        var act = () => { Email email = nullEmail; };

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithParameterName("Email address cannot be null or white space");
    }

    [Theory]
    [InlineData("notanemail")]
    [InlineData("missing@domain")]
    public void When_Invalid_Email_String_Is_Implicitly_Converted_Then_Throw_InvalidEmailException(string invalidEmail)
    {
        // Arrange & Act
        var act = () => { Email email = invalidEmail; };

        // Assert
        act.Should().Throw<InvalidEmailException>();
    }
}
