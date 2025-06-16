using MediatR;
using Application;
using ServiceExeption.Exceptions;

namespace Application;

public class DeleteCommentHandler(ICommentStore commentStore, ICdnProvider cdnProvider, ICurrentUserService currentUser) : IRequestHandler<DeleteCommentCommand>
{
    public async Task Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var entity = await commentStore.GetById(request.Id);
        if (entity == null)
            throw new NotFoundException("Post not found");

        var currentUserId = currentUser.Id;
        var isAdmin = currentUser.IsInRole("Admin");

        if (entity.AuthorId != currentUserId && !isAdmin)
            throw new UnauthorizedException("Access denied");

        await commentStore.Delete(request.Id);

        await cdnProvider.DeleteFilesAsync(entity.Medias.ToList());
    }
}
