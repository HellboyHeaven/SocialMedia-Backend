using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/posts")]
public class PostController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
        => Ok(await mediator.Send(new GetPostByIdCommand(id)));

    [HttpGet]
    public async Task<IActionResult> GetPosts([FromQuery] int page = 1)
        => Ok(await mediator.Send(new GetPostsCommand(page)));

    [HttpGet("by-username/{username}")]
    public async Task<IActionResult> GetPostsByUsername(string username, [FromQuery] int page = 1)
            => Ok(await mediator.Send(new GetPostsByUsernameCommand(username, page)));

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromForm] CreatePostRequest request)
    {
        return Ok(await mediator.Send(new CreatePostCommand(request.Content, request.Medias)));
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Edit(Guid id, [FromForm] EditPostRequest request)
    {
        return Ok(await mediator.Send(new EditPostCommand(id, request.Content, request.OldMedias, request.NewMedias)));
    }


    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        await mediator.Send(new DeletePostCommand(id));
        return Ok();
    }
}
