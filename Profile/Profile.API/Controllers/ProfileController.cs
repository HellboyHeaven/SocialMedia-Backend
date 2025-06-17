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
        => Ok(await mediator.Send(new GetMyBriefProfileCommand()));

    [HttpGet("{username}")]
    public async Task<IActionResult> GetByUsername(string Username)
        => Ok(await mediator.Send(new GetProfileByUsernameCommand(Username)));


    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateProfileRequest request)
    {
        var command = new CreateProfileCommand
        (
            request.Username,
            request.Nickname,
            request.Description,
            request.Avatar
        );
        return Ok(await mediator.Send(command));
    }


}
