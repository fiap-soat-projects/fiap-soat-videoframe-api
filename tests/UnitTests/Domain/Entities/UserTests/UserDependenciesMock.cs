using Domain.Entities;
using Domain.ValueObjects;

namespace UnitTests.Domain.Entities.UserTests;

public abstract class UserDependenciesMock
{
    protected readonly User _sut;

    protected UserDependenciesMock(
        string? userId = "user-123",
        string? userName = "John Doe",
        string? emailAddress = "john@example.com")
    {
        var email = new Email(emailAddress ?? "john@example.com");
        _sut = new User(userId ?? "user-123", userName ?? "John Doe", email);
    }
}
