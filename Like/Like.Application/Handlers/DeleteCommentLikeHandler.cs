using MediatR;
using Core;

namespace Application;

public class DeleteCommentLikeHandler(ILikeStore likeStore) : IRequestHandler<DeleteCommentLikeCommand>
{
    public async Task Handle(DeleteCommentLikeCommand request, CancellationToken cancellationToken)
    {
        await likeStore.Delete<CommentLike>(request.AuthorId, request.CommentId);
    }
}
