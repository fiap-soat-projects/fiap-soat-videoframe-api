using Api.Authentication.Extensions;
using Api.Authentication.Interfaces;
using System.Security.Claims;

namespace Api.Authentication;

public class UserContextMocked : IUserContext
{
    public string Id { get; init; }
    public string Name { get; init; }
    public string Email { get; init; }

    public UserContextMocked(IHttpContextAccessor context)
    {
        Id = "1";
        Name = "name";
        Email = "teste@test.com";
    }
}