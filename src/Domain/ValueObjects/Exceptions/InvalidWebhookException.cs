using Domain.Entities.Exceptions;

namespace Domain.ValueObjects.Exceptions;

public class InvalidWebhookException : DomainException
{
    const string INVALID_WEBHOOK_MESSAGE_TEMPLATE = "The webhook address {0} is invalid";
    public InvalidWebhookException(string address) : base(string.Format(INVALID_WEBHOOK_MESSAGE_TEMPLATE, address))
    {

    }
}
