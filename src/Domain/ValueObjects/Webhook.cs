using Domain.ValueObjects.Exceptions;
using System.Text.RegularExpressions;

namespace Domain.ValueObjects;

public readonly partial struct Webhook
{
    public string Adress { get; }

    public static implicit operator Webhook(string? value) => new(value);
    public static implicit operator string(Webhook email) => email.Adress;

    private static readonly Regex EmailRegex = new Regex(@"^(https?:\/\/)?([\w\-]+\.)+[\w\-]+(\/[\w\-._~:/?#[\]@!$&'()*+,;=]*)?$", RegexOptions.Compiled);

    public Webhook(string? address)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(address, "Webhook address cannot be null or white space");

        if (EmailRegex.IsMatch(address) is false)
        {
            throw new InvalidWebhookException(address);
        }

        Adress = address;
    }

    public override string ToString()
    {
        return Adress;
    }
}
