using Domain.Entities;
using FluentAssertions;

namespace UnitTests.Domain.Entities.VideoTests.Methods;

public class CreatedAtPropertyTests : VideoDependenciesMock
{
    [Fact]
    public void When_CreatedAt_Is_Get_Then_Returns_Current_DateTime_UTC()
    {
        // Act
        var result = _sut.CreatedAt;

        // Assert
        result.Kind.Should().Be(DateTimeKind.Utc);
        result.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void When_CreatedAt_Is_Set_With_Valid_DateTime_Then_Updates_Property()
    {
        // Arrange
        var newCreatedAt = new DateTime(2024, 6, 15, 10, 30, 0, DateTimeKind.Utc);

        // Act
        _sut.CreatedAt = newCreatedAt;

        // Assert
        _sut.CreatedAt.Should().Be(newCreatedAt);
    }

    [Theory]
    [InlineData("2024-01-01T00:00:00Z")]
    [InlineData("2023-12-25T18:45:30Z")]
    [InlineData("2025-06-15T12:30:45Z")]
    public void When_CreatedAt_Is_Set_With_Different_Valid_Dates_Then_Updates_Property(string dateString)
    {
        // Arrange
        var newCreatedAt = DateTime.Parse(dateString, null, System.Globalization.DateTimeStyles.AdjustToUniversal);

        // Act
        _sut.CreatedAt = newCreatedAt;

        // Assert
        _sut.CreatedAt.Should().Be(newCreatedAt);
    }

    [Fact]
    public void When_CreatedAt_Is_Changed_Multiple_Times_Then_Property_Reflects_Latest_Value()
    {
        // Arrange
        var date1 = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc);
        var date2 = new DateTime(2024, 6, 15, 18, 30, 0, DateTimeKind.Utc);
        var date3 = new DateTime(2024, 12, 31, 23, 59, 59, DateTimeKind.Utc);

        // Act
        _sut.CreatedAt = date1;
        _sut.CreatedAt = date2;
        _sut.CreatedAt = date3;

        // Assert
        _sut.CreatedAt.Should().Be(date3);
    }

    [Fact]
    public void When_Video_Is_Created_With_Explicit_CreatedAt_Then_Uses_Provided_Value()
    {
        // Arrange
        var expectedCreatedAt = new DateTime(2024, 3, 10, 15, 45, 30, DateTimeKind.Utc);

        // Act
        var video = new Video("id-123", expectedCreatedAt, "user-123", "/path", "name", "video/mp4", 1024);

        // Assert
        video.CreatedAt.Should().Be(expectedCreatedAt);
    }
}
