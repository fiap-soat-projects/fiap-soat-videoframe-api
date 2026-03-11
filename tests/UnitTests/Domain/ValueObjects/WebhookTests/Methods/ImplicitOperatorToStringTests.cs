using Domain.ValueObjects;
using FluentAssertions;

namespace UnitTests.Domain.ValueObjects.WebhookTests.Methods;

public class ImplicitOperatorToStringTests
{
    [Theory]
    [InlineData("https://example.com")]
    [InlineData("http://example.com/webhook")]
    [InlineData("https://api.example.com/v1/webhook")]
    public void When_Webhook_Is_Implicitly_Converted_To_String_Then_Returns_Address(string webhookAddress)
    {
        // Arrange
        var webhook = new Webhook(webhookAddress);

        // Act
        string result = webhook;

        // Assert
        result.Should().Be(webhookAddress);
    }

    [Fact]
    public void When_Webhook_Is_Implicitly_Converted_To_String_Then_Same_As_Property()
    {
        // Arrange
        var webhookAddress = "https://example.com/webhook";
        var webhook = new Webhook(webhookAddress);

        // Act
        string result = webhook;

        // Assert
        result.Should().Be(webhook.Adress);
    }
}
