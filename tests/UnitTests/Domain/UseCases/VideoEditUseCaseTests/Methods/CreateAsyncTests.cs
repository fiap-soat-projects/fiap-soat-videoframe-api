using Domain.Entities;
using Domain.Entities.Enums;
using FluentAssertions;
using NSubstitute;

namespace UnitTests.Domain.UseCases.VideoEditUseCaseTests.Methods;

public class CreateAsyncTests : VideoEditUseCaseDependenciesMock
{
    [Fact]
    public async Task When_CreateAsync_Then_Returns_Id_From_Repository()
    {
        // Arrange
        var expectedId = "id-123";
        var videoEdit = new VideoEdit("userId", "recipient", EditType.Frame, EditStatus.Created, "videoId", []);

        _videoEditRepository.InsertOneAsync(videoEdit, Arg.Any<CancellationToken>()).Returns(expectedId);

        // Act
        var result = await _sut.CreateAsync(videoEdit, CancellationToken.None);

        // Assert
        result.Should().Be(expectedId);
        await _videoEditRepository.Received(1).InsertOneAsync(videoEdit, Arg.Any<CancellationToken>());
    }
}
