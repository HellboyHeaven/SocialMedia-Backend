using MediatR;
using Core;

namespace Application;

public class CreateProfileHandler(IProfileStore postStore, ICdnProvider cdnProvider, ICurrentUserService currentUser) : IRequestHandler<CreateProfileCommand, Guid>
{

    public async Task<Guid> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
    {
        string? avatarUrl = null;
        if (request.Avatar != null)
        {
            avatarUrl = await cdnProvider.UploadFileAsync(request.Avatar);
        }
        var entity = new ProfileEntity
        {
            UserId = currentUser.Id,
            Username = request.Username,
            Nickname = request.Nickname,
            Description = request.Description,
            Avatar = avatarUrl
        };
        var id = await postStore.Create(entity);
        // await kafkaProducer.SendMessageAsync(ACTION, id.ToString(), entity);
        return id;
    }
}
