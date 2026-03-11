using Domain.Entities;
using Domain.Entities.Exceptions;
using FluentAssertions;

namespace UnitTests.Domain.Entities.VideoTests.Methods;

public class IdPropertyTests : VideoDependenciesMock
{
    [Fact]
    public void When_Id_Is_Get_On_Video_Without_Id_Then_Returns_Null()
    {
        // Act
        var result = _sut.Id;

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void When_Id_Is_Set_With_Valid_Value_Then_Updates_Property()
    {
        // Arrange
        var newId = "video-456";

        // Act
        _sut.Id = newId;

        // Assert
        _sut.Id.Should().Be(newId);
    }

    [Theory]
    [InlineData("new-id-1")]
    [InlineData("video-xyz")]
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
            .Throw<InvalidEntityPropertyException<Video>>();
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
            .Throw<InvalidEntityPropertyException<Video>>();
    }

    [Fact]
    public void When_Id_Is_Changed_Multiple_Times_Then_Property_Reflects_Latest_Value()
    {
        // Arrange
        var id1 = "video-first";
        var id2 = "video-second";
        var id3 = "video-third";

        // Act
        _sut.Id = id1;
        _sut.Id = id2;
        _sut.Id = id3;

        // Assert
        _sut.Id.Should().Be(id3);
    }
}
