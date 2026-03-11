using Domain.Entities;
using Domain.Entities.Enums;
using FluentAssertions;

namespace UnitTests.Domain.Entities.VideoEditTests.Methods;

public class StatusPropertyTests : VideoEditDependenciesMock
{
    [Fact]
    public void When_Status_Is_Get_Then_Returns_Stored_Value()
    {
        // Act
        var result = _sut.Status;

        // Assert
        result.Should().Be(EditStatus.Created);
    }

    [Fact]
    public void When_Status_Is_Set_With_Valid_Value_Then_Updates_Property()
    {
        // Arrange
        var newStatus = EditStatus.Processing;

        // Act
        _sut.Status = newStatus;

        // Assert
        _sut.Status.Should().Be(newStatus);
    }

    [Theory]
    [InlineData(EditStatus.None)]
    [InlineData(EditStatus.Created)]
    [InlineData(EditStatus.Processing)]
    [InlineData(EditStatus.Processed)]
    [InlineData(EditStatus.Sending)]
    [InlineData(EditStatus.Sent)]
    [InlineData(EditStatus.Error)]
    public void When_Status_Is_Set_With_Different_Valid_Values_Then_Updates_Property(EditStatus newStatus)
    {
        // Act
        _sut.Status = newStatus;

        // Assert
        _sut.Status.Should().Be(newStatus);
    }

    [Fact]
    public void When_Status_Is_Changed_Multiple_Times_Then_Property_Reflects_Latest_Value()
    {
        // Arrange
        var status1 = EditStatus.Created;
        var status2 = EditStatus.Processing;
        var status3 = EditStatus.Processed;

        // Act
        _sut.Status = status1;
        _sut.Status = status2;
        _sut.Status = status3;

        // Assert
        _sut.Status.Should().Be(status3);
    }
}
