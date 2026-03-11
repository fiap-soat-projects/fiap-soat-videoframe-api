using Domain.ValueObjects;
using Domain.ValueObjects.Exceptions;
using FluentAssertions;

namespace UnitTests.Domain.ValueObjects.EmailTests.Constructor;

public class EmailConstructorTests
{
    [Theory]
    [InlineData("user@example.com")]
    [InlineData("test.email@domain.co.uk")]
    [InlineData("name+tag@company.com")]
    [InlineData("simple@test.net")]
    public void When_Valid_Email_Address_Then_Construction_Succeeds(string validEmail)
    {
        // Arrange & Act
        var email = new Email(validEmail);

        // Assert
        email.Adress.Should().Be(validEmail);
    }

    [Fact]
    public void When_Email_Is_Null_Then_Throw_ArgumentException()
    {
        // Arrange
        string? nullEmail = null;

        // Act
        var act = () => new Email(nullEmail);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithParameterName("Email address cannot be null or white space");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public void When_Email_Is_Whitespace_Then_Throw_ArgumentException(string whitespaceEmail)
    {
        // Arrange & Act
        var act = () => new Email(whitespaceEmail);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithParameterName("Email address cannot be null or white space");
    }

    [Theory]
    [InlineData("notanemail")]
    [InlineData("missing@domain")]
    [InlineData("@domain.com")]
    [InlineData("user@")]
    [InlineData("user @example.com")]
    [InlineData("user@ example.com")]
    [InlineData("user@exam ple.com")]
    public void When_Email_Format_Is_Invalid_Then_Throw_InvalidEmailException(string invalidEmail)
    {
        // Arrange & Act
        var act = () => new Email(invalidEmail);

        // Assert
        act.Should()
            .Throw<InvalidEmailException>();
    }
}
