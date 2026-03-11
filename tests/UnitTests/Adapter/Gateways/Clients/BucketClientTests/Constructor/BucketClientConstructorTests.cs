using Adapter.Gateways.Clients;
using Infrastructure.Clients.Interfaces;
using FluentAssertions;
using NSubstitute;

namespace UnitTests.Adapter.Gateways.Clients.BucketClientTests.Constructor;

public class BucketClientConstructorTests
{
    [Fact]
    public void When_Valid_IS3BucketClient_Is_Provided_Then_Construction_Succeeds()
    {
        // Arrange
        var s3BucketClientMock = Substitute.For<IS3BucketClient>();

        // Act
        var client = new BucketClient(s3BucketClientMock);

        // Assert
        client.Should().NotBeNull();
    }
}
