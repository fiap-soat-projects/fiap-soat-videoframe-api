using Business.ValueObjects.Exceptions;
using System.Text.RegularExpressions;

namespace Domain.ValueObjects;

public readonly partial struct Email
{
    public string Adress { get; }

    public static implicit operator Email(string? value) => new(value);
    public static implicit operator string(Email email) => email.Adress;

    private static readonly Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    public Email(string? address)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(address, "Email address cannot be null or white space");

        if (EmailRegex.IsMatch(address) is false)
        {
            throw new InvalidEmailException(address);
        }

        Adress = address;
    }

    public override string ToString()
    {
        return Adress;
    }
}
