namespace Api.Authentication.Interfaces;

public interface IUserContext
{
    string Id { get; }
    string Email { get; }
    string Name { get; }
}