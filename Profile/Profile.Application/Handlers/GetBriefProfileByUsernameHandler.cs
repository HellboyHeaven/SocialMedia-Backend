using MediatR;

namespace Application;

public partial class GetProfileByUsernameHandler
{
    public class GetBriefProfileByUsernameHandler(IProfileStore profileStore) : IRequestHandler<GetBriefProfileByIdCommand, BriefProfileModel?>
    {
        public async Task<BriefProfileModel?> Handle(GetBriefProfileByIdCommand request, CancellationToken cancellationToken)
        {
            var entity = await profileStore.GetByUserId(request.UserId);
            if (entity == null) return null;
            var model = new BriefProfileModel
            {
                Username = entity.Username,
                Nickname = entity.Nickname,
                Avatar = entity.Avatar,
            };
            return model;
        }
    }
}
