using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

[ApiController]
[Route("api/auth")]
public class AuthController(IMediator mediator) : ControllerBase
{

    [HttpGet("auth-check")]
    public IActionResult AuthCheck()
    {
        var name = User.Identity?.Name;
        var claims = User.Claims.Select(c => new { c.Type, c.Value });
        var token = HttpContext.Request.Headers["Authorization"].ToString();
        return Ok(token);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
        => Ok(await mediator.Send(new LoginUserCommand(request.Login, request.Password, HttpContext.Request.Headers.UserAgent!)));

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] LoginRequest request)
    {
        await mediator.Send(new RegisterUserCommand(request.Login, request.Password));
        return Ok();
    }

    [Authorize]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        return Ok(await mediator.Send(new RefreshTokenCommand(token)));
    }

    [Authorize]
    [HttpPost("revoke-refreshtoken")]
    public async Task<IActionResult> RevokeRefreshToken()
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var userAgent = HttpContext.Request.Headers.UserAgent;
        Console.WriteLine($"User {userId} revoked token for {userAgent}");
        await mediator.Send(new RevokeTokenCommand(new Guid(userId), userAgent!));
        return Ok();
    }
}
