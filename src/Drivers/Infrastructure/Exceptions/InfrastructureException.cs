using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Exceptions;

[ExcludeFromCodeCoverage]
public abstract class InfrastructureException : Exception
{
    public InfrastructureException(string? message) : base(message)
    {

    }
}
