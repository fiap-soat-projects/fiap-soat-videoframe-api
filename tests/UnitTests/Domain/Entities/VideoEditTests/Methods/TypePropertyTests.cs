using Domain.Entities;
using Domain.Entities.Enums;
using FluentAssertions;

namespace UnitTests.Domain.Entities.VideoEditTests.Methods;

public class TypePropertyTests : VideoEditDependenciesMock
{
    [Fact]
    public void When_Type_Is_Get_Then_Returns_Stored_Value()
    {
        // Act
        var result = _sut.Type;

        // Assert
        result.Should().Be(EditType.Frame);
    }

    [Fact]
    public void When_Type_Is_Set_With_Valid_Value_Then_Updates_Property()
    {
        // Arrange
        var newType = EditType.None;

        // Act
        _sut.Type = newType;

        // Assert
        _sut.Type.Should().Be(newType);
    }

    [Theory]
    [InlineData(EditType.None)]
    [InlineData(EditType.Frame)]
    public void When_Type_Is_Set_With_Different_Valid_Values_Then_Updates_Property(EditType newType)
    {
        // Act
        _sut.Type = newType;

        // Assert
        _sut.Type.Should().Be(newType);
    }

    [Fact]
    public void When_Type_Is_Changed_Multiple_Times_Then_Property_Reflects_Latest_Value()
    {
        // Arrange
        var type1 = EditType.None;
        var type2 = EditType.Frame;
        var type3 = EditType.None;

        // Act
        _sut.Type = type1;
        _sut.Type = type2;
        _sut.Type = type3;

        // Assert
        _sut.Type.Should().Be(type3);
    }
}
