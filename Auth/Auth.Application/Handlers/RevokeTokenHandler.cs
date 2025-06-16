using MediatR;
using Core;

namespace Application;

public class RevokeTokenHandler(IRefreshTokenStore refreshTokenStore) : IRequestHandler<RevokeTokenCommand>
{
    public async Task Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
    {
        var refrehTokenEntity = await refreshTokenStore.GetByUserIdAndAgent(request.UserId, request.UserAgent);
        if (refrehTokenEntity == null) return;
        await refreshTokenStore.Delete(refrehTokenEntity);
    }
}
