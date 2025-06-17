using Application;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;


public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public Guid Id
    {
        get
        {
            var userIdStr = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (Guid.TryParse(userIdStr, out var userId))
                return userId;

            throw new UnauthorizedAccessException("User is not authenticated or has invalid ID.");
        }
    }

    public bool Exists =>
        Guid.TryParse(httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier), out _);

    public bool IsInRole(string role) =>
        httpContextAccessor.HttpContext?.User?.IsInRole(role) ?? false;
}
