using Api.Authentication.Exceptions;

namespace Api.Authentication.Extensions;

public static class HttpContextAcessorExtensions
{
    extension(IHttpContextAccessor httpContextAcessor)
    {
        public string GetRequiredUserClaim(string claimName)
        {
            var value = httpContextAcessor.HttpContext?.User?.FindFirst(claimName)?.Value;

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidUserPropertyException(claimName);
            }

            return value;
        }
    }
}
