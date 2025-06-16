using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/likes/comments/{commentId:guid}")]
public class CommentLikeController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(Guid commentId)
    {
        var userId = GetUserId();
        var command = new CreateCommentLikeCommand(userId, commentId);
        await mediator.Send(command);
        return Created();
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid commentId)
    {
        var userId = GetUserId();
        var command = new DeleteCommentLikeCommand(userId, commentId);
        await mediator.Send(command);
        return Ok();
    }

    private Guid GetUserId()
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        return Guid.Parse(userId);
    }
}
