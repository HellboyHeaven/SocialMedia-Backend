using MediatR;
using Core;
using ServiceExeption.Exceptions;

namespace Application;

public class CreateCommentHandler(ICommentStore commentStore, ICdnProvider cdnProvider, IMessageManager messageManager, ICurrentUserService currentUser) : IRequestHandler<CreateCommentCommand, CommentModel>
{

    public async Task<CommentModel> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        IEnumerable<string> medias = await cdnProvider.UploadFilesAsync(request.Medias);
        var profileExists = await messageManager.GetProfileExists(currentUser.Id);
        var postExists = await messageManager.GetPostExists(request.PostId);
        if (!profileExists)
            throw new NotFoundException($"Profile with id {currentUser.Id} not found");
        if (!postExists)
            throw new NotFoundException($"Post with id {request.PostId} not found");

        var entity = new CommentEntity
        {
            AuthorId = currentUser.Id,
            PostId = request.PostId,
            Content = request.Content,
            Medias = medias
        };
        var id = await commentStore.Create(entity);

        var model = new CommentModel
        {
            Id = id,
            Author = await messageManager.GetProfileById(currentUser.Id),
            Content = entity.Content,
            Medias = entity.Medias,
            CreatedAt = entity.CreatedAt
        };

        return model;
    }
}
