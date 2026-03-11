using Domain.ValueObjects;
using Domain.ValueObjects.Exceptions;
using FluentAssertions;

namespace UnitTests.Domain.ValueObjects.WebhookTests.Methods;

public class ImplicitOperatorFromStringTests
{
    [Theory]
    [InlineData("https://example.com")]
    [InlineData("http://example.com/webhook")]
    [InlineData("https://api.example.com/v1/webhook")]
    public void When_Valid_String_Is_Implicitly_Converted_To_Webhook_Then_Returns_Webhook(string webhookString)
    {
        // Arrange & Act
        Webhook webhook = webhookString;

        // Assert
        webhook.Adress.Should().Be(webhookString);
    }

    [Fact]
    public void When_Null_String_Is_Implicitly_Converted_To_Webhook_Then_Throw_ArgumentException()
    {
        // Arrange
        string? nullWebhook = null;

        // Act
        var act = () => { Webhook webhook = nullWebhook; };

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithParameterName("Webhook address cannot be null or white space");
    }

    [Theory]
    [InlineData("not a valid url")]
    [InlineData("ftp://example.com")]
    public void When_Invalid_Webhook_String_Is_Implicitly_Converted_Then_Throw_InvalidWebhookException(string invalidWebhook)
    {
        // Arrange & Act
        var act = () => { Webhook webhook = invalidWebhook; };

        // Assert
        act.Should().Throw<InvalidWebhookException>();
    }
}
