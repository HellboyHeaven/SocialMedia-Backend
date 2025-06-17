using MediatR;

namespace Application;

public class GetMyBriefProfileHandler(IProfileStore profileStore, ICurrentUserService currentUser) : IRequestHandler<GetMyBriefProfileCommand, BriefProfileModel?>
{
    public async Task<BriefProfileModel?> Handle(GetMyBriefProfileCommand request, CancellationToken cancellationToken)
    {
        var entity = await profileStore.GetByUserId(currentUser.Id);
        if (entity == null) return null;
        var model = new BriefProfileModel
        {
            Username = entity.Username,
            Nickname = entity.Nickname,
            Avatar = entity.Avatar,
            IsAdmin = currentUser.IsInRole("Admin")
        };
        return model;
    }
}
