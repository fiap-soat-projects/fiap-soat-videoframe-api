using Domain.ValueObjects;

namespace UnitTests.Domain.ValueObjects.EmailTests;

public abstract class EmailDependenciesMock
{
    protected readonly Email _sut;

    protected EmailDependenciesMock(string validEmail = "test@example.com")
    {
        _sut = new Email(validEmail);
    }
}
