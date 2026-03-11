using Domain.Entities;
using Domain.Entities.Exceptions;
using FluentAssertions;

namespace UnitTests.Domain.Entities.VideoEditTests.Methods;

public class VideoIdPropertyTests : VideoEditDependenciesMock
{
    [Fact]
    public void When_VideoId_Is_Get_Then_Returns_Stored_Value()
    {
        // Act
        var result = _sut.VideoId;

        // Assert
        result.Should().Be("video-123");
    }

    [Fact]
    public void When_VideoId_Is_Set_With_Valid_Value_Then_Updates_Property()
    {
        // Arrange
        var newVideoId = "video-456";

        // Act
        _sut.VideoId = newVideoId;

        // Assert
        _sut.VideoId.Should().Be(newVideoId);
    }

    [Theory]
    [InlineData("video-1")]
    [InlineData("video-abc-xyz")]
    [InlineData("123456")]
    public void When_VideoId_Is_Set_With_Different_Valid_Values_Then_Updates_Property(string newVideoId)
    {
        // Act
        _sut.VideoId = newVideoId;

        // Assert
        _sut.VideoId.Should().Be(newVideoId);
    }

    [Fact]
    public void When_VideoId_Is_Set_To_Null_Then_Throw_InvalidEntityPropertyException()
    {
        // Act
        var act = () => _sut.VideoId = null;

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
    public void When_VideoId_Is_Set_To_Whitespace_Then_Throw_InvalidEntityPropertyException(string whitespaceVideoId)
    {
        // Act
        var act = () => _sut.VideoId = whitespaceVideoId;

        // Assert
        act.Should()
            .Throw<InvalidEntityPropertyException<VideoEdit>>();
    }

    [Fact]
    public void When_VideoId_Is_Changed_Multiple_Times_Then_Property_Reflects_Latest_Value()
    {
        // Arrange
        var videoId1 = "video-first";
        var videoId2 = "video-second";
        var videoId3 = "video-third";

        // Act
        _sut.VideoId = videoId1;
        _sut.VideoId = videoId2;
        _sut.VideoId = videoId3;

        // Assert
        _sut.VideoId.Should().Be(videoId3);
    }
}
