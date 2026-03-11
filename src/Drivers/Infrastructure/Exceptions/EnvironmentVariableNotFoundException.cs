using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Exceptions;

[ExcludeFromCodeCoverage]
public class EnvironmentVariableNotFoundException : InfrastructureException
{
    public EnvironmentVariableNotFoundException(string? message) : base(message)
    {

    }

    public static void ThrowIfIsNullOrWhiteSpace(string? variableValue, string variableName)
    {
        if (string.IsNullOrWhiteSpace(variableValue))
        {
            throw new EnvironmentVariableNotFoundException($"Environment variable {variableName} name cannot be null or whitespace");
        }
    }
}
