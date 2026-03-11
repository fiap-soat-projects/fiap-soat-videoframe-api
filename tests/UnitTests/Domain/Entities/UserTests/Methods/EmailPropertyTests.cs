using Domain.Entities;
using Domain.ValueObjects;
using FluentAssertions;

namespace UnitTests.Domain.Entities.UserTests.Methods;

public class EmailPropertyTests : UserDependenciesMock
{
    [Fact]
    public void When_Email_Is_Get_Then_Returns_Stored_Value()
    {
        // Act
        var result = _sut.Email;

        // Assert
        result.Should().Be(new Email("john@example.com"));
    }

    [Fact]
    public void When_Email_Is_Set_With_Valid_Email_Then_Updates_Property()
    {
        // Arrange
        var newEmail = new Email("jane@example.com");

        // Act
        _sut.Email = newEmail;

        // Assert
        _sut.Email.Should().Be(newEmail);
    }

    [Theory]
    [InlineData("alice@test.com")]
    [InlineData("bob@domain.org")]
    [InlineData("charlie@company.net")]
    public void When_Email_Is_Set_With_Different_Valid_Emails_Then_Updates_Property(string emailAddress)
    {
        // Arrange
        var newEmail = new Email(emailAddress);

        // Act
        _sut.Email = newEmail;

        // Assert
        _sut.Email.Should().Be(newEmail);
    }

    [Fact]
    public void When_Email_Is_Changed_Multiple_Times_Then_Property_Reflects_Latest_Value()
    {
        // Arrange
        var email1 = new Email("first@example.com");
        var email2 = new Email("second@example.com");
        var email3 = new Email("third@example.com");

        // Act
        _sut.Email = email1;
        _sut.Email = email2;
        _sut.Email = email3;

        // Assert
        _sut.Email.Should().Be(email3);
    }

    [Fact]
    public void When_Email_Is_Retrieved_Then_Same_As_Constructed_Value()
    {
        // Arrange
        var email = new Email("test@example.com");
        var user = new User("user-id", "User Name", email);

        // Act
        var result = user.Email;

        // Assert
        result.Should().Be(email);
    }
}
