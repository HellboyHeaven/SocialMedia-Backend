using MediatR;

namespace Application;

public class GetProfileByUsernameHandler(IProfileStore profileStore) : IRequestHandler<GetProfileByUsernameCommand, ProfileModel?>
{
    public async Task<ProfileModel?> Handle(GetProfileByUsernameCommand request, CancellationToken cancellationToken)
    {
        var entity = await profileStore.GetByUsername(request.Username);
        if (entity == null) return null;
        var model = new ProfileModel
        {
            Username = entity.Username,
            Nickname = entity.Nickname,
            Description = entity.Description,
            Avatar = entity.Avatar,
        };
        return model;
    }
}
