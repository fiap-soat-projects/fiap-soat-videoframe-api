using Domain.Entities;
using Domain.Entities.Exceptions;
using FluentAssertions;

namespace UnitTests.Domain.Entities.VideoTests.Methods;

public class PathPropertyTests : VideoDependenciesMock
{
    [Fact]
    public void When_Path_Is_Get_Then_Returns_Stored_Value()
    {
        // Act
        var result = _sut.Path;

        // Assert
        result.Should().Be("/videos/test.mp4");
    }

    [Fact]
    public void When_Path_Is_Set_With_Valid_Value_Then_Updates_Property()
    {
        // Arrange
        var newPath = "/videos/new-video.mp4";

        // Act
        _sut.Path = newPath;

        // Assert
        _sut.Path.Should().Be(newPath);
    }

    [Theory]
    [InlineData("/videos/video1.mp4")]
    [InlineData("/folder/subfolder/video.mkv")]
    [InlineData("/path/to/file.avi")]
    public void When_Path_Is_Set_With_Different_Valid_Values_Then_Updates_Property(string newPath)
    {
        // Act
        _sut.Path = newPath;

        // Assert
        _sut.Path.Should().Be(newPath);
    }

    [Fact]
    public void When_Path_Is_Set_To_Null_Then_Throw_InvalidEntityPropertyException()
    {
        // Act
        var act = () => _sut.Path = null;

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
    public void When_Path_Is_Set_To_Whitespace_Then_Throw_InvalidEntityPropertyException(string whitespacePath)
    {
        // Act
        var act = () => _sut.Path = whitespacePath;

        // Assert
        act.Should()
            .Throw<InvalidEntityPropertyException<Video>>();
    }

    [Fact]
    public void When_Path_Is_Changed_Multiple_Times_Then_Property_Reflects_Latest_Value()
    {
        // Arrange
        var path1 = "/videos/path1.mp4";
        var path2 = "/videos/path2.mp4";
        var path3 = "/videos/path3.mp4";

        // Act
        _sut.Path = path1;
        _sut.Path = path2;
        _sut.Path = path3;

        // Assert
        _sut.Path.Should().Be(path3);
    }
}
