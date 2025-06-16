using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/likes/posts/{postId:guid}")]
public class PostLikeController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(Guid postId)
    {
        var userId = GetUserId();
        var command = new CreatePostLikeCommand(userId, postId);
        await mediator.Send(command);
        return Created();
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid postId)
    {
        var userId = GetUserId();
        var command = new DeletePostLikeCommand(userId, postId);
        await mediator.Send(command);
        return Ok();
    }

    private Guid GetUserId()
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        return Guid.Parse(userId);
    }
}
