using Adapter.Presenters.DTOs;
using Domain.Entities.Enums;
using FluentAssertions;
using NSubstitute;

namespace UnitTests.Adapter.Controllers.VideoEditControllerTests.Methods;

public class GetLinkAsyncTests : VideoEditControllerDependenciesMock
{
    [Fact]
    public async Task When_Valid_Id_And_UserRequest_Then_Returns_VideoLinkPresenter()
    {
        // Arrange
        var id = "edit-123";
        var userId = "user-123";
        var userRequest = new UserRequest(userId, "John Doe", "recipient@example.com");
        var expectedLink = "https://presigned-url.example.com/edit";

        _videoEditUseCaseMock.GetLinkAsync(id, userId, Arg.Any<CancellationToken>())
            .Returns(expectedLink);

        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _sut.GetLinkAsync(id, userRequest, cancellationToken);

        // Assert
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData("id-1", "user-1", "https://link1.example.com")]
    [InlineData("id-abc", "user-abc", "https://link2.example.com")]
    [InlineData("id-xyz", "user-xyz", "https://link3.example.com")]
    public async Task When_Different_Ids_And_UserIds_Then_Calls_UseCase_With_Correct_Parameters(string id, string userId, string expectedLink)
    {
        // Arrange
        var userRequest = new UserRequest(userId, "John Doe", "recipient@example.com");
        var cancellationToken = new CancellationToken();

        _videoEditUseCaseMock.GetLinkAsync(id, userId, cancellationToken).Returns(expectedLink);

        // Act
        var result = await _sut.GetLinkAsync(id, userRequest, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        await _videoEditUseCaseMock.Received(1).GetLinkAsync(id, userId, cancellationToken);
    }

    [Fact]
    public async Task When_UseCase_Throws_Exception_Then_Propagates_Exception()
    {
        // Arrange
        var id = "edit-123";
        var userId = "user-123";
        var userRequest = new UserRequest(userId, "John Doe", "recipient@example.com");
        var expectedException = new InvalidOperationException("Link generation failed");

        _videoEditUseCaseMock.GetLinkAsync(id, userId, Arg.Any<CancellationToken>())
            .Returns(Task.FromException<string>(expectedException));

        var cancellationToken = CancellationToken.None;

        // Act
        var act = async () => await _sut.GetLinkAsync(id, userRequest, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
