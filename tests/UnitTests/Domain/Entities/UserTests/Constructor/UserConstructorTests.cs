using Domain.Entities;
using Domain.Entities.Exceptions;
using Domain.ValueObjects;
using FluentAssertions;

namespace UnitTests.Domain.Entities.UserTests.Constructor;

public class UserConstructorTests
{
    [Fact]
    public void When_Valid_Parameters_Then_Construction_Succeeds()
    {
        // Arrange
        var userId = "user-123";
        var userName = "John Doe";
        var email = new Email("john@example.com");

        // Act
        var user = new User(userId, userName, email);

        // Assert
        user.Id.Should().Be(userId);
        user.Name.Should().Be(userName);
        user.Email.Should().Be(email);
    }

    [Theory]
    [InlineData("user-1", "John Doe", "john@example.com")]
    [InlineData("user-abc-xyz", "Jane Smith", "jane@test.com")]
    [InlineData("123456", "Alice Johnson", "alice@domain.com")]
    public void When_Different_Valid_Parameters_Then_Construction_Succeeds(
        string userId, 
        string userName, 
        string emailAddress)
    {
        // Arrange
        var email = new Email(emailAddress);

        // Act
        var user = new User(userId, userName, email);

        // Assert
        user.Id.Should().Be(userId);
        user.Name.Should().Be(userName);
        user.Email.Should().Be(email);
    }

    [Fact]
    public void When_Id_Is_Null_Then_Throw_InvalidEntityPropertyException()
    {
        // Arrange
        string? nullId = null;
        var email = new Email("john@example.com");

        // Act
        var act = () => new User(nullId, "John Doe", email);

        // Assert
        act.Should()
            .Throw<InvalidEntityPropertyException<User>>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public void When_Id_Is_Whitespace_Then_Throw_InvalidEntityPropertyException(string whitespaceId)
    {
        // Arrange
        var email = new Email("john@example.com");

        // Act
        var act = () => new User(whitespaceId, "John Doe", email);

        // Assert
        act.Should()
            .Throw<InvalidEntityPropertyException<User>>();
    }

    [Fact]
    public void When_Name_Is_Null_Then_Throw_InvalidEntityPropertyException()
    {
        // Arrange
        string? nullName = null;
        var email = new Email("john@example.com");

        // Act
        var act = () => new User("user-123", nullName, email);

        // Assert
        act.Should()
            .Throw<InvalidEntityPropertyException<User>>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public void When_Name_Is_Whitespace_Then_Throw_InvalidEntityPropertyException(string whitespaceName)
    {
        // Arrange
        var email = new Email("john@example.com");

        // Act
        var act = () => new User("user-123", whitespaceName, email);

        // Assert
        act.Should()
            .Throw<InvalidEntityPropertyException<User>>();
    }
}
