using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Exceptions;

[ExcludeFromCodeCoverage]
public class DuplicateItemException : InfrastructureException
{
    public DuplicateItemException(string? message) : base(message)
    {

    }
}
