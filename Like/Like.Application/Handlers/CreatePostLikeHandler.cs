using MediatR;
using Core;
using ServiceExeption.Exceptions;

namespace Application;

public class CreatePostLikeHandler(ILikeStore likeStore, IMessageManager messageManager) : IRequestHandler<CreatePostLikeCommand>
{
    public async Task Handle(CreatePostLikeCommand request, CancellationToken cancellationToken)
    {
        var profileExists = await messageManager.ProfileExists(request.AuthorId);
        var postExists = await messageManager.PostExists(request.PostId);

        if (!profileExists)
            throw new NotFoundException($"Profile with id {request.AuthorId} not found");
        if (!postExists)
            throw new NotFoundException($"Post with id {request.PostId} not found");

        var entity = new PostLike
        {
            AuthorId = request.AuthorId,
            PostId = request.PostId,
            CreatedAt = DateTime.UtcNow
        };

        await likeStore.Create(entity);
    }
}
