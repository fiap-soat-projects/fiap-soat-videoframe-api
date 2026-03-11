using Domain.Entities;
using Domain.Entities.Enums;
using FluentAssertions;

namespace UnitTests.Domain.Entities.VideoEditTests.Methods;

public class EditPathPropertyTests : VideoEditDependenciesMock
{
    [Fact]
    public void When_EditPath_Is_Get_Then_Returns_Null_For_VideoEdit_Without_Path()
    {
        // Act
        var result = _sut.EditPath;

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void When_EditPath_Is_Set_With_Valid_Value_Then_Updates_Property()
    {
        // Arrange
        var newEditPath = "/edits/new-path";

        // Act
        _sut.EditPath = newEditPath;

        // Assert
        _sut.EditPath.Should().Be(newEditPath);
    }

    [Theory]
    [InlineData("/edits/path1")]
    [InlineData("/folder/subfolder/edit")]
    [InlineData("/path/to/file.mp4")]
    public void When_EditPath_Is_Set_With_Different_Valid_Values_Then_Updates_Property(string newEditPath)
    {
        // Act
        _sut.EditPath = newEditPath;

        // Assert
        _sut.EditPath.Should().Be(newEditPath);
    }

    [Fact]
    public void When_EditPath_Is_Set_To_Null_Then_Updates_Property()
    {
        // Arrange
        _sut.EditPath = "/some/path";

        // Act
        _sut.EditPath = null;

        // Assert
        _sut.EditPath.Should().BeNull();
    }

    [Fact]
    public void When_EditPath_Is_Changed_Multiple_Times_Then_Property_Reflects_Latest_Value()
    {
        // Arrange
        var path1 = "/edits/path1";
        var path2 = "/edits/path2";
        var path3 = "/edits/path3";

        // Act
        _sut.EditPath = path1;
        _sut.EditPath = path2;
        _sut.EditPath = path3;

        // Assert
        _sut.EditPath.Should().Be(path3);
    }

    [Fact]
    public void When_VideoEdit_Is_Created_With_Explicit_EditPath_Then_Uses_Provided_Value()
    {
        // Arrange
        var expectedEditPath = "/edits/edit-123";

        // Act
        var videoEdit = new VideoEdit(
            "id-123",
            DateTime.UtcNow,
            "user-123",
            "recipient@example.com",
            EditType.Frame,
            EditStatus.Created,
            "video-123",
            expectedEditPath,
            new List<NotificationTarget>());

        // Assert
        videoEdit.EditPath.Should().Be(expectedEditPath);
    }
}
