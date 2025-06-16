using MediatR;
using ServiceExeption.Exceptions;

namespace Application;

public class GetPostsByUsernameHandler(IPostStore postStore, IMessageManager messageManager, ICurrentUserService currentUser) : IRequestHandler<GetPostsByUsernameCommand, List<PostModel>>
{
    public async Task<List<PostModel>> Handle(GetPostsByUsernameCommand request, CancellationToken cancellationToken)
    {
        var authorId = await messageManager.GetUserIdAsync(request.Username, cancellationToken);
        if (authorId == null)
            throw new NotFoundException($"User {request.Username} not found");

        var posts = await postStore.GetAllByAuthorId(authorId.Value, request.Page, 10);

        var tasks = posts.Select(async post =>
        {
            var authorTask = messageManager.GetProfileAsync(post.AuthorId, cancellationToken);
            var commentCountTask = messageManager.GetCommentCountAsync(post.Id, cancellationToken);
            var likeCountTask = messageManager.GetLikeCountAsync(post.Id, cancellationToken);
            var likedTask = currentUser.Exists ? messageManager.LikeExistsAsync(post.Id, currentUser.Id, cancellationToken) : Task.FromResult(false);

            await Task.WhenAll(authorTask, commentCountTask, likeCountTask, likedTask);

            return new PostModel
            {
                Id = post.Id,
                Content = post.Content,
                Author = await authorTask,
                Medias = post.Medias,
                Comments = await commentCountTask,
                Likes = await likeCountTask,
                Liked = await likedTask,
                CreatedAt = post.CreatedAt,
                EditedAt = post.EditedAt,
            };
        });

        return (await Task.WhenAll(tasks)).ToList();
    }
}
