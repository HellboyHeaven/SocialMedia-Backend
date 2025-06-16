using ServiceExeption.Exceptions;
using MediatR;

namespace Application;

public class GetPostByIdHandler(IPostStore postStore, IMessageManager messageManager, ICurrentUserService currentUser) : IRequestHandler<GetPostByIdCommand, PostModel?>
{
    public async Task<PostModel?> Handle(GetPostByIdCommand request, CancellationToken cancellationToken)
    {
        var entity = await postStore.GetById(request.PostId);
        if (entity == null) throw new NotFoundException("Post not found");

        var profileTask = messageManager.GetProfileAsync(entity.AuthorId, cancellationToken);
        var commentsTask = messageManager.GetCommentCountAsync(entity.Id, cancellationToken);
        var likesTask = messageManager.GetLikeCountAsync(entity.Id, cancellationToken);
        var likedTask = currentUser.Exists
            ? messageManager.LikeExistsAsync(entity.Id, currentUser.Id, cancellationToken)
            : Task.FromResult(false);

        await Task.WhenAll(profileTask, commentsTask, likesTask, likedTask);

        var profileModel = await profileTask;
        var comments = await commentsTask;
        var likes = await likesTask;
        var liked = await likedTask;

        var model = new PostModel
        {
            Id = entity.Id,
            Author = profileModel,
            Content = entity.Content,
            Medias = entity.Medias,
            CreatedAt = entity.CreatedAt,
            EditedAt = entity.EditedAt,
            Comments = comments,
            Likes = likes,
            Liked = liked
        };

        return model;
    }
}
