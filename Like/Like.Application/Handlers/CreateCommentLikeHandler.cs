using MediatR;
using Core;
using ServiceExeption.Exceptions;

namespace Application;

public class CreateCommentLikeHandler(ILikeStore likeStore, IMessageManager messageManager) : IRequestHandler<CreateCommentLikeCommand>
{
    public async Task Handle(CreateCommentLikeCommand request, CancellationToken cancellationToken)
    {
        var profileExists = await messageManager.ProfileExists(request.AuthorId);
        var commentExists = await messageManager.CommentExists(request.CommentId);

        if (!profileExists)
            throw new NotFoundException($"Profile with id {request.AuthorId} not found");
        if (!commentExists)
            throw new NotFoundException($"Comment with id {request.CommentId} not found");

        var entity = new CommentLike
        {
            AuthorId = request.AuthorId,
            CommentId = request.CommentId,
            CreatedAt = DateTime.UtcNow
        };

        await likeStore.Create(entity);
    }
}
