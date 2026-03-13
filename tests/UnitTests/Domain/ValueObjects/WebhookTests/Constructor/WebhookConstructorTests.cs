using Domain.ValueObjects;
using Domain.ValueObjects.Exceptions;
using FluentAssertions;

namespace UnitTests.Domain.ValueObjects.WebhookTests.Constructor;

public class WebhookConstructorTests
{
    [Theory]
    [InlineData("https://example.com")]
    [InlineData("http://example.com")]
    [InlineData("https://example.com/webhook")]
    [InlineData("https://sub.example.com/path")]
    [InlineData("https://example.com/path?query=value")]
    [InlineData("example.com")]
    [InlineData("example.com/webhook")]
    public void When_Valid_Webhook_Address_Then_Construction_Succeeds(string validWebhook)
    {
        // Arrange & Act
        var webhook = new Webhook(validWebhook);

        // Assert
        webhook.Adress.Should().Be(validWebhook);
    }

    [Fact]
    public void When_Webhook_Is_Null_Then_Throw_ArgumentException()
    {
        // Arrange
        string? nullWebhook = null;

        // Act
        var act = () => new Webhook(nullWebhook);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithParameterName("Webhook address cannot be null or white space");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public void When_Webhook_Is_Whitespace_Then_Throw_ArgumentException(string whitespaceWebhook)
    {
        // Arrange & Act
        var act = () => new Webhook(whitespaceWebhook);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithParameterName("Webhook address cannot be null or white space");
    }

    [Theory]
    [InlineData("not a valid url")]
    [InlineData("ftp://example.com")]
    [InlineData("://example.com")]
    [InlineData("https://")]
    [InlineData("https://.com")]    
    [InlineData("htp://example.com")]
    public void When_Webhook_Format_Is_Invalid_Then_Throw_InvalidWebhookException(string invalidWebhook)
    {
        // Arrange & Act
        var act = () => new Webhook(invalidWebhook);

        // Assert
        act.Should()
            .Throw<InvalidWebhookException>();
    }
}
