using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/comments")]
public class CommentController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
        => Ok(await mediator.Send(new GetCommentByIdCommand(id)));

    [HttpGet]
    public async Task<IActionResult> GetAllByPostId([FromQuery] Guid postId, int page = 1)
        => Ok(await mediator.Send(new GetCommentsByPostIdCommand(postId, page)));

    [HttpGet("by-username/{username}")]
    public async Task<IActionResult> GetPostsByUsername(string username, [FromQuery] int page = 1)
        => Ok(await mediator.Send(new GetCommentsByUsernameCommand(username, page)));

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateCommentRequest request)
    {
        var command = new CreateCommentCommand(request.PostId, request.Content, request.Medias);
        return Ok(await mediator.Send(command));
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Edit(Guid id, [FromForm] EditCommentRequest request)
    {
        return Ok(await mediator.Send(new EditCommentCommand(id, request.Content, request.OldMedias, request.NewMedias)));
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        await mediator.Send(new DeleteCommentCommand(id));
        return Ok();
    }

    // private Guid? GetUserIdOrNull()
    // {
    //     var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
    //     if (userIdClaim == null)
    //         return null;

    //     Console.WriteLine($"User ID: {userIdClaim.Value}");
    //     return Guid.Parse(userIdClaim.Value);
    // }
}
