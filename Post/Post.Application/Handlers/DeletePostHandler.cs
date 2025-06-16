using MediatR;
using Core;
using ServiceExeption.Exceptions;

namespace Application;

public class DeletePostHandler(IPostStore postStore, ICdnProvider cdnProvider, ICurrentUserService currentUser) : IRequestHandler<DeletePostCommand>
{
    public async Task Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var post = await postStore.GetById(request.Id);
        if (post == null)
            throw new NotFoundException("Post not found");

        var currentUserId = currentUser.Id;
        var isAdmin = currentUser.IsInRole("Admin");

        if (post.AuthorId != currentUserId && !isAdmin)
            throw new UnauthorizedException("Access denied");

        await postStore.Delete(request.Id);

        await cdnProvider.DeleteFilesAsync(post.Medias.ToList());
    }
}
