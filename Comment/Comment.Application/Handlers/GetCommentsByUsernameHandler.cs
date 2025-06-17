using MediatR;
using ServiceExeption.Exceptions;

namespace Application;

public class GetCommentsByUsernameHandler(ICommentStore commentStore, IMessageManager messageManager, ICurrentUserService currentUser) : IRequestHandler<GetCommentsByUsernameCommand, List<CommentWithPostIdModel>>
{
    public async Task<List<CommentWithPostIdModel>> Handle(GetCommentsByUsernameCommand request, CancellationToken cancellationToken)
    {
        var authorId = await messageManager.GetUserIdAsync(request.Username);
        if (authorId == null)
            throw new NotFoundException($"User {request.Username} not found");

        var comments = await commentStore.GetAllByAuthorId(authorId.Value, request.Page, 10);

        var tasks = comments.Select(async comment =>
        {
            var authorTask = messageManager.GetProfileById(comment.AuthorId);
            var likeCountTask = messageManager.GetLikeCount(comment.Id);
            var likedTask = currentUser.Exists ? messageManager.GetLikeExists(comment.Id, currentUser.Id) : Task.FromResult(false);
            await Task.WhenAll(authorTask, likeCountTask, likedTask);
            return new CommentWithPostIdModel
            {
                Id = comment.Id,
                PostId = comment.PostId,
                Content = comment.Content,
                Author = await authorTask,
                Likes = await likeCountTask,
                Liked = await likedTask,
                CreatedAt = comment.CreatedAt,
                EditedAt = comment.EditedAt,
            };
        });

        return (await Task.WhenAll(tasks)).ToList();
    }
}
