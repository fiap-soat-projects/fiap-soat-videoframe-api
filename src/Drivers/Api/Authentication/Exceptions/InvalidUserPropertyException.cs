namespace Api.Authentication.Exceptions;

public class InvalidUserPropertyException : Exception
{
    private const string DEFAULT_MESSAGE = "Invalid user properties";

    public string PropertyName { get; init; }

    public InvalidUserPropertyException(string propertyName) : base(DEFAULT_MESSAGE)
    {
        PropertyName = propertyName;
    }
}
