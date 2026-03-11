using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Entities.Exceptions;
using FluentAssertions;

namespace UnitTests.Domain.Entities.VideoEditTests.Methods;

public class UserIdPropertyTests : VideoEditDependenciesMock
{
    [Fact]
    public void When_UserId_Is_Get_Then_Returns_Stored_Value()
    {
        // Act
        var result = _sut.UserId;

        // Assert
        result.Should().Be("user-123");
    }

    [Fact]
    public void When_UserId_Is_Set_With_Valid_Value_Then_Updates_Property()
    {
        // Arrange
        var newUserId = "user-456";

        // Act
        _sut.UserId = newUserId;

        // Assert
        _sut.UserId.Should().Be(newUserId);
    }

    [Theory]
    [InlineData("user-1")]
    [InlineData("user-abc-xyz")]
    [InlineData("123456")]
    public void When_UserId_Is_Set_With_Different_Valid_Values_Then_Updates_Property(string newUserId)
    {
        // Act
        _sut.UserId = newUserId;

        // Assert
        _sut.UserId.Should().Be(newUserId);
    }

    [Fact]
    public void When_UserId_Is_Set_To_Null_Then_Throw_InvalidEntityPropertyException()
    {
        // Act
        var act = () => _sut.UserId = null;

        // Assert
        act.Should()
            .Throw<InvalidEntityPropertyException<VideoEdit>>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public void When_UserId_Is_Set_To_Whitespace_Then_Throw_InvalidEntityPropertyException(string whitespaceUserId)
    {
        // Act
        var act = () => _sut.UserId = whitespaceUserId;

        // Assert
        act.Should()
            .Throw<InvalidEntityPropertyException<VideoEdit>>();
    }

    [Fact]
    public void When_UserId_Is_Changed_Multiple_Times_Then_Property_Reflects_Latest_Value()
    {
        // Arrange
        var userId1 = "user-first";
        var userId2 = "user-second";
        var userId3 = "user-third";

        // Act
        _sut.UserId = userId1;
        _sut.UserId = userId2;
        _sut.UserId = userId3;

        // Assert
        _sut.UserId.Should().Be(userId3);
    }
}
