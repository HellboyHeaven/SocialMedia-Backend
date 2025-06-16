using MediatR;

namespace Application;

public class GetCommentsByPostIdHandler(ICommentStore commentStore, IMessageManager messageManager, ICurrentUserService currentUser) : IRequestHandler<GetCommentsByPostIdCommand, List<CommentModel>>
{
    public async Task<List<CommentModel>> Handle(GetCommentsByPostIdCommand request, CancellationToken cancellationToken)
    {
        var comments = await commentStore.GetAllByPostId(request.PostId, request.Page, 10);

        var tasks = comments.Select(async comment =>
        {
            var authorTask = messageManager.GetProfileById(comment.AuthorId);
            var likeCountTask = messageManager.GetLikeCount(comment.Id);
            var likedTask = currentUser.Exists ? messageManager.GetLikeExists(comment.Id, currentUser.Id) : Task.FromResult(false);

            await Task.WhenAll(authorTask, likeCountTask, likedTask);

            return new CommentModel
            {
                Id = comment.Id,
                Content = comment.Content,
                Author = await authorTask,
                Medias = comment.Medias,
                Likes = await likeCountTask,
                Liked = await likedTask,
                CreatedAt = comment.CreatedAt
            };
        });

        return (await Task.WhenAll(tasks)).ToList();
    }
}
