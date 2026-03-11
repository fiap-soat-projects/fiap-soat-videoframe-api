using Domain.ValueObjects;
using FluentAssertions;

namespace UnitTests.Domain.ValueObjects.WebhookTests.Methods;

public class ToStringTests : WebhookDependenciesMock
{
    [Theory]
    [InlineData("https://example.com")]
    [InlineData("http://example.com/webhook")]
    [InlineData("https://api.example.com/v1/webhook")]
    public void When_ToString_Is_Called_Then_Returns_Address(string webhookAddress)
    {
        // Arrange
        var webhook = new Webhook(webhookAddress);

        // Act
        var result = webhook.ToString();

        // Assert
        result.Should().Be(webhookAddress);
    }

    [Fact]
    public void When_ToString_Is_Called_Then_Same_As_Property()
    {
        // Arrange
        var webhookAddress = "https://example.com/webhook";
        var webhook = new Webhook(webhookAddress);

        // Act
        var result = webhook.ToString();

        // Assert
        result.Should().Be(webhook.Adress);
    }

    [Fact]
    public void When_ToString_Is_Called_On_Sut_Then_Returns_Valid_Address()
    {
        // Act
        var result = _sut.ToString();

        // Assert
        result.Should().Be("https://example.com/webhook");
    }
}
