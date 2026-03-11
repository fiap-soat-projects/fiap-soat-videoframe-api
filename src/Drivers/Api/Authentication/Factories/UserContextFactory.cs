using Api.Authentication.Exceptions;
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

        ValidateUserProperties(cognitoUser);

        return new UserContext(cognitoUser.Id!, cognitoUser.Name!, cognitoUser.Email!);
    }

    private static void ValidateUserProperties(Infrastructure.Clients.DTOs.CognitoUser cognitoUser)
    {
        if (string.IsNullOrEmpty(cognitoUser.Id))
        {
            throw new InvalidUserPropertyException(nameof(cognitoUser.Id));
        }

        if (string.IsNullOrEmpty(cognitoUser.Name))
        {
            throw new InvalidUserPropertyException(nameof(cognitoUser.Id));
        }

        if (string.IsNullOrEmpty(cognitoUser.Email))
        {
            throw new InvalidUserPropertyException(nameof(cognitoUser.Id));
        }
    }
}
