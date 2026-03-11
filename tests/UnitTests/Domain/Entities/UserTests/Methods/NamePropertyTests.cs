using Domain.Entities;
using Domain.Entities.Exceptions;
using Domain.ValueObjects;
using FluentAssertions;

namespace UnitTests.Domain.Entities.UserTests.Methods;

public class NamePropertyTests : UserDependenciesMock
{
    [Fact]
    public void When_Name_Is_Get_Then_Returns_Stored_Value()
    {
        // Act
        var result = _sut.Name;

        // Assert
        result.Should().Be("John Doe");
    }

    [Fact]
    public void When_Name_Is_Set_With_Valid_Value_Then_Updates_Property()
    {
        // Arrange
        var newName = "Jane Smith";

        // Act
        _sut.Name = newName;

        // Assert
        _sut.Name.Should().Be(newName);
    }

    [Theory]
    [InlineData("Alice Johnson")]
    [InlineData("Bob Wilson")]
    [InlineData("Charlie Brown")]
    public void When_Name_Is_Set_With_Different_Valid_Values_Then_Updates_Property(string newName)
    {
        // Act
        _sut.Name = newName;

        // Assert
        _sut.Name.Should().Be(newName);
    }

    [Fact]
    public void When_Name_Is_Set_To_Null_Then_Throw_InvalidEntityPropertyException()
    {
        // Act
        var act = () => _sut.Name = null;

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
    public void When_Name_Is_Set_To_Whitespace_Then_Throw_InvalidEntityPropertyException(string whitespaceName)
    {
        // Act
        var act = () => _sut.Name = whitespaceName;

        // Assert
        act.Should()
            .Throw<InvalidEntityPropertyException<User>>();
    }

    [Fact]
    public void When_Name_Is_Changed_Multiple_Times_Then_Property_Reflects_Latest_Value()
    {
        // Arrange
        var name1 = "First Name";
        var name2 = "Second Name";
        var name3 = "Third Name";

        // Act
        _sut.Name = name1;
        _sut.Name = name2;
        _sut.Name = name3;

        // Assert
        _sut.Name.Should().Be(name3);
    }
}
