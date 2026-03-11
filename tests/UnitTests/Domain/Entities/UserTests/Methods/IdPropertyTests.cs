using Domain.Entities;
using Domain.Entities.Exceptions;
using Domain.ValueObjects;
using FluentAssertions;

namespace UnitTests.Domain.Entities.UserTests.Methods;

public class IdPropertyTests : UserDependenciesMock
{
    [Fact]
    public void When_Id_Is_Get_Then_Returns_Stored_Value()
    {
        // Act
        var result = _sut.Id;

        // Assert
        result.Should().Be("user-123");
    }

    [Fact]
    public void When_Id_Is_Set_With_Valid_Value_Then_Updates_Property()
    {
        // Arrange
        var newId = "user-456";

        // Act
        _sut.Id = newId;

        // Assert
        _sut.Id.Should().Be(newId);
    }

    [Theory]
    [InlineData("new-id-1")]
    [InlineData("user-xyz")]
    [InlineData("123456789")]
    public void When_Id_Is_Set_With_Different_Valid_Values_Then_Updates_Property(string newId)
    {
        // Act
        _sut.Id = newId;

        // Assert
        _sut.Id.Should().Be(newId);
    }

    [Fact]
    public void When_Id_Is_Set_To_Null_Then_Throw_InvalidEntityPropertyException()
    {
        // Act
        var act = () => _sut.Id = null;

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
    public void When_Id_Is_Set_To_Whitespace_Then_Throw_InvalidEntityPropertyException(string whitespaceId)
    {
        // Act
        var act = () => _sut.Id = whitespaceId;

        // Assert
        act.Should()
            .Throw<InvalidEntityPropertyException<User>>();
    }

    [Fact]
    public void When_Id_Is_Changed_Multiple_Times_Then_Property_Reflects_Latest_Value()
    {
        // Arrange
        var id1 = "user-first";
        var id2 = "user-second";
        var id3 = "user-third";

        // Act
        _sut.Id = id1;
        _sut.Id = id2;
        _sut.Id = id3;

        // Assert
        _sut.Id.Should().Be(id3);
    }
}
