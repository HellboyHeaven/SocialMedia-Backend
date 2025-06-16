using MediatR;

namespace Application;

public class GetCommentByIdHandler(ICommentStore commentStore, IMessageManager messageManager, ICurrentUserService currentUser) : IRequestHandler<GetCommentByIdCommand, CommentModel?>
{
    public async Task<CommentModel?> Handle(GetCommentByIdCommand request, CancellationToken cancellationToken)
    {
        var entity = await commentStore.GetById(request.Id);
        if (entity == null) return null;
        var profileModel = await messageManager.GetProfileById(entity.AuthorId);
        var likeCount = await messageManager.GetLikeCount(entity.Id);
        var isLiked = currentUser.Exists ? false : await messageManager.GetLikeExists(entity.Id, currentUser.Id);
        var model = new CommentModel
        {
            Id = entity.Id,
            Author = profileModel!,
            Content = entity.Content,
            CreatedAt = entity.CreatedAt,
            Likes = likeCount,
            Liked = isLiked,
        };

        return model;
    }


}
