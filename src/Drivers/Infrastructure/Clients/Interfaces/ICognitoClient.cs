using Infrastructure.Clients.DTOs;

namespace Infrastructure.Clients.Interfaces;

public interface ICognitoClient
{
    Task<CognitoUser> GetUserAsync(string id, CancellationToken cancellationToken);
}
