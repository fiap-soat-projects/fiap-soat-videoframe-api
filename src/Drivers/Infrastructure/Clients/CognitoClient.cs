using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Infrastructure.Clients.DTOs;
using Infrastructure.Clients.Interfaces;

namespace Infrastructure.Clients;

public class CognitoClient : ICognitoClient
{
    private readonly AmazonCognitoIdentityProviderClient _amazonCognitoIdentityProviderClient;

    public CognitoClient(AmazonCognitoIdentityProviderClient amazonCognitoIdentityProviderClient)
    {
        _amazonCognitoIdentityProviderClient = amazonCognitoIdentityProviderClient;
    }

    public async Task<CognitoUser> GetUserAsync(string accessToken, CancellationToken cancellationToken)
    {
        var request = new GetUserRequest
        {
            AccessToken = accessToken
        };

        var response = await _amazonCognitoIdentityProviderClient.GetUserAsync(request);

        var email = response.UserAttributes
            .FirstOrDefault(a => a.Name == "custom:user_email")?.Value;

        var userId = response.UserAttributes
            .FirstOrDefault(a => a.Name == "custom:user_id")?.Value;

        var userName = response.UserAttributes
            .FirstOrDefault(a => a.Name == "custom:user_name")?.Value;

        var user = new CognitoUser
        {
            Id = userId,
            Name = userName,
            Email = email
        };

        return user;
    }
}
