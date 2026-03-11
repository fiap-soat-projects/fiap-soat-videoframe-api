using Domain.Entities;
using Domain.Entities.Exceptions;
using FluentAssertions;

namespace UnitTests.Domain.Entities.VideoTests.Methods;

public class NamePropertyTests : VideoDependenciesMock
{
    [Fact]
    public void When_Name_Is_Get_Then_Returns_Stored_Value()
    {
        // Act
        var result = _sut.Name;

        // Assert
        result.Should().Be("test-video.mp4");
    }

    [Fact]
    public void When_Name_Is_Set_With_Valid_Value_Then_Updates_Property()
    {
        // Arrange
        var newName = "new-video.mp4";

        // Act
        _sut.Name = newName;

        // Assert
        _sut.Name.Should().Be(newName);
    }

    [Theory]
    [InlineData("video1.mp4")]
    [InlineData("movie.mkv")]
    [InlineData("file.avi")]
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
            .Throw<InvalidEntityPropertyException<Video>>();
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
            .Throw<InvalidEntityPropertyException<Video>>();
    }

    [Fact]
    public void When_Name_Is_Changed_Multiple_Times_Then_Property_Reflects_Latest_Value()
    {
        // Arrange
        var name1 = "name1.mp4";
        var name2 = "name2.mp4";
        var name3 = "name3.mp4";

        // Act
        _sut.Name = name1;
        _sut.Name = name2;
        _sut.Name = name3;

        // Assert
        _sut.Name.Should().Be(name3);
    }
}
