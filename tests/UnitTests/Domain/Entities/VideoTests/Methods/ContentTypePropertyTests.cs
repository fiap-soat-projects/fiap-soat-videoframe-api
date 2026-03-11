using Domain.Entities;
using Domain.Entities.Exceptions;
using FluentAssertions;

namespace UnitTests.Domain.Entities.VideoTests.Methods;

public class ContentTypePropertyTests : VideoDependenciesMock
{
    [Fact]
    public void When_ContentType_Is_Get_Then_Returns_Stored_Value()
    {
        // Act
        var result = _sut.ContentType;

        // Assert
        result.Should().Be("video/mp4");
    }

    [Fact]
    public void When_ContentType_Is_Set_With_Valid_Value_Then_Updates_Property()
    {
        // Arrange
        var newContentType = "video/x-matroska";

        // Act
        _sut.ContentType = newContentType;

        // Assert
        _sut.ContentType.Should().Be(newContentType);
    }

    [Theory]
    [InlineData("video/mp4")]
    [InlineData("video/x-matroska")]
    [InlineData("video/x-msvideo")]
    [InlineData("video/webm")]
    [InlineData("video/quicktime")]
    public void When_ContentType_Is_Set_With_Different_Valid_Values_Then_Updates_Property(string newContentType)
    {
        // Act
        _sut.ContentType = newContentType;

        // Assert
        _sut.ContentType.Should().Be(newContentType);
    }

    [Fact]
    public void When_ContentType_Is_Set_To_Null_Then_Throw_InvalidEntityPropertyException()
    {
        // Act
        var act = () => _sut.ContentType = null;

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
    public void When_ContentType_Is_Set_To_Whitespace_Then_Throw_InvalidEntityPropertyException(string whitespaceContentType)
    {
        // Act
        var act = () => _sut.ContentType = whitespaceContentType;

        // Assert
        act.Should()
            .Throw<InvalidEntityPropertyException<Video>>();
    }

    [Fact]
    public void When_ContentType_Is_Changed_Multiple_Times_Then_Property_Reflects_Latest_Value()
    {
        // Arrange
        var contentType1 = "video/mp4";
        var contentType2 = "video/x-matroska";
        var contentType3 = "video/x-msvideo";

        // Act
        _sut.ContentType = contentType1;
        _sut.ContentType = contentType2;
        _sut.ContentType = contentType3;

        // Assert
        _sut.ContentType.Should().Be(contentType3);
    }
}
