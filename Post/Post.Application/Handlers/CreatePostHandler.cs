using MediatR;
using Core;

namespace Application;

public class CreatePostHandler(IPostStore postStore, ICdnProvider cdnProvider, IMessageManager messageManager, ICurrentUserService currentUser) : IRequestHandler<CreatePostCommand, PostModel>
{
    public async Task<PostModel> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        List<string> medias = await cdnProvider.UploadFilesAsync(request.Medias);

        var entity = new PostEntity
        {
            AuthorId = currentUser.Id,
            Content = request.Content,
            Medias = medias.ToArray(),
            CreatedAt = DateTime.UtcNow
        };
        var id = await postStore.Create(entity);

        var model = new PostModel
        {
            Id = id,
            Author = await messageManager.GetProfileAsync(currentUser.Id),
            Content = entity.Content,
            Medias = entity.Medias,
            CreatedAt = entity.CreatedAt
        };

        return model;
    }
}
