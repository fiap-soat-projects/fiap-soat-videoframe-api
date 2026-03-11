using Domain.Entities;
using Domain.Entities.Exceptions;
using FluentAssertions;

namespace UnitTests.Domain.Entities.VideoTests.Methods;

public class ContentLengthPropertyTests : VideoDependenciesMock
{
    [Fact]
    public void When_ContentLength_Is_Get_Then_Returns_Stored_Value()
    {
        // Act
        var result = _sut.ContentLength;

        // Assert
        result.Should().Be(1024000);
    }

    [Fact]
    public void When_ContentLength_Is_Set_With_Valid_Value_Then_Updates_Property()
    {
        // Arrange
        var newContentLength = 2048000L;

        // Act
        _sut.ContentLength = newContentLength;

        // Assert
        _sut.ContentLength.Should().Be(newContentLength);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(1024)]
    [InlineData(1024000)]
    [InlineData(5000000)]
    [InlineData(long.MaxValue)]
    public void When_ContentLength_Is_Set_With_Different_Valid_Values_Then_Updates_Property(long newContentLength)
    {
        // Act
        _sut.ContentLength = newContentLength;

        // Assert
        _sut.ContentLength.Should().Be(newContentLength);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-1000)]
    [InlineData(long.MinValue)]
    public void When_ContentLength_Is_Set_To_Zero_Or_Lower_Then_Throw_InvalidEntityPropertyException(long invalidContentLength)
    {
        // Act
        var act = () => _sut.ContentLength = invalidContentLength;

        // Assert
        act.Should()
            .Throw<InvalidEntityPropertyException<Video>>();
    }

    [Fact]
    public void When_ContentLength_Is_Changed_Multiple_Times_Then_Property_Reflects_Latest_Value()
    {
        // Arrange
        var contentLength1 = 1024000L;
        var contentLength2 = 2048000L;
        var contentLength3 = 5000000L;

        // Act
        _sut.ContentLength = contentLength1;
        _sut.ContentLength = contentLength2;
        _sut.ContentLength = contentLength3;

        // Assert
        _sut.ContentLength.Should().Be(contentLength3);
    }

    [Fact]
    public void When_ContentLength_Is_One_Then_Property_Updates_Successfully()
    {
        // Arrange
        var minValidContentLength = 1L;

        // Act
        _sut.ContentLength = minValidContentLength;

        // Assert
        _sut.ContentLength.Should().Be(minValidContentLength);
    }
}
