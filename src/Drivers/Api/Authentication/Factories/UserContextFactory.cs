using Api.Authentication.Interfaces;
using Infrastructure.Clients.Interfaces;

namespace Api.Authentication.Factories;

public static class UserContextFactory
{
    public static IUserContext Create(IServiceProvider services)
    {
        var context = services.GetService<IHttpContextAccessor>();
        var client = services.GetService<ICognitoClient>();

        var authHeader = context!
            .HttpContext?
            .Request
            .Headers["Authorization"]
            .FirstOrDefault();

        var token = authHeader?.Replace("Bearer ", "");

        if (string.IsNullOrEmpty(authHeader) || string.IsNullOrEmpty(token))
        {
            return new UserContext(string.Empty, string.Empty, string.Empty);
        }

        var cognitoUser = client!.GetUserAsync(token!, CancellationToken.None).GetAwaiter().GetResult();

        return new UserContext(
            cognitoUser.Id ?? string.Empty,
            cognitoUser.Name ?? string.Empty,
            cognitoUser.Email ?? string.Empty);
    }
}
