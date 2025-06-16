using MediatR;

namespace Application;

public class GetPostsHandler(IPostStore postStore, IMessageManager messageManager, ICurrentUserService currentUser) : IRequestHandler<GetPostsCommand, List<PostModel>>
{
    public async Task<List<PostModel>> Handle(GetPostsCommand request, CancellationToken cancellationToken)
    {
        var posts = await postStore.GetAll(request.page, 10);

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
