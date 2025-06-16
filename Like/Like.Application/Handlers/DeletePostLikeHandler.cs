using MediatR;
using Core;

namespace Application;

public class DeletePostLikeHandler(ILikeStore likeStore) : IRequestHandler<DeletePostLikeCommand>
{
    public async Task Handle(DeletePostLikeCommand request, CancellationToken cancellationToken)
    {
        await likeStore.Delete<PostLike>(request.AuthorId, request.PostId);
    }
}
