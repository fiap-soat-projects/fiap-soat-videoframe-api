using Api.Authentication.Extensions;
using Api.Authentication.Interfaces;
using System.Security.Claims;

namespace Api.Authentication;

public class UserContext : IUserContext
{
    public string Id { get; init; }
    public string Name { get; init; }
    public string Email { get; init; }

    public UserContext(IHttpContextAccessor context)
    {
        Id = context.GetRequiredUserClaim("userId");
        Name = context.GetRequiredUserClaim("name");
        Email = context.GetRequiredUserClaim(ClaimTypes.Email);
    }
}