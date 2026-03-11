using Domain.Entities.Enums;
using NSubstitute;

namespace UnitTests.Domain.UseCases.VideoEditUseCaseTests.Methods;

public class UpdateStatusAsyncTests : VideoEditUseCaseDependenciesMock
{
    [Theory]
    [InlineData("id123", 3)]
    public async Task When_Called_Then_Repository_UpdateStatus_Is_Invoked(string id, int statusValue)
    {
        // Arrange
        var status = (EditStatus)statusValue;

        _videoEditRepository.UpdateStatusAsync(id, Arg.Any<string>(), status, Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        // Act
        await _sut.UpdateStatusAsync(id, status, "userId", CancellationToken.None);

        // Assert
        await _videoEditRepository.Received(1).UpdateStatusAsync(id, "userId", status, Arg.Any<CancellationToken>());
    }
}
