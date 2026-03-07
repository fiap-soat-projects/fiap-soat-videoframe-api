using Api.Authentication.Interfaces;
using System.Security.Claims;

namespace Api.Authentication;

public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _context;

    public UserContext(IHttpContextAccessor context)
    {
        _context = context;
    }

    public string? UserId =>
        _context.HttpContext?.User?.FindFirst("userId")?.Value;

    public string? Email =>
        _context.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;

    public string? Name =>
        _context.HttpContext?.User?.FindFirst("name")?.Value;
}