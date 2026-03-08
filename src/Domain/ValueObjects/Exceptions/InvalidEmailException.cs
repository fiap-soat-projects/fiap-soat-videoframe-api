using Domain.Entities.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace Business.ValueObjects.Exceptions;

[ExcludeFromCodeCoverage]
public class InvalidEmailException : DomainException
{
    const string INVALID_EMAIL_MESSAGE_TEMPLATE = "The email address {0} is invalid";

    public InvalidEmailException(string address) : base(string.Format(INVALID_EMAIL_MESSAGE_TEMPLATE, address))
    {

    }
}
