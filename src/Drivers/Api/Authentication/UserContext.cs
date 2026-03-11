using Api.Authentication.Interfaces;

namespace Api.Authentication;

public class UserContext(string id, string name, string email) : IUserContext
{
    public string Id { get; init; } = id;
    public string Name { get; init; } = name;
    public string Email { get; init; } = email;
}