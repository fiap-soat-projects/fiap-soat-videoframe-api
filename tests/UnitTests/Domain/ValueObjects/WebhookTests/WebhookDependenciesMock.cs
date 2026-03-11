using Domain.ValueObjects;

namespace UnitTests.Domain.ValueObjects.WebhookTests;

public abstract class WebhookDependenciesMock
{
    protected readonly Webhook _sut;

    protected WebhookDependenciesMock(string validWebhook = "https://example.com/webhook")
    {
        _sut = new Webhook(validWebhook);
    }
}
