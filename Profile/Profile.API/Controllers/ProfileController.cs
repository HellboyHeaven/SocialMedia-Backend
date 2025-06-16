using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/profiles")]
public class ProfileController(IMediator mediator) : ControllerBase
{
    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me()
        => Ok(GetUserIdOrNull() == null ? null : await mediator.Send(new GetBriefProfileByIdCommand(GetUserIdOrNull()!.Value)));


    [HttpGet("{username}")]
    public async Task<IActionResult> GetByUsername(string Username)
        => Ok(await mediator.Send(new GetProfileByUsernameCommand(Username)));

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateProfileRequest request)
    {
        var userId = GetUserIdOrNull();
        var command = new CreateProfileCommand
        (
            userId!.Value,
            request.Username,
            request.Nickname,
            request.Description,
            request.Avatar
        );
        return Ok(await mediator.Send(command));
    }

    private Guid? GetUserIdOrNull()
    {
        var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            return null;

        Console.WriteLine($"User ID: {userIdClaim.Value}");
        return Guid.Parse(userIdClaim.Value);
    }
}
