using MediatR;
using Core;
using ServiceExeption.Exceptions;

namespace Application;

public class RefreshTokenHandler(IRefreshTokenStore refreshTokenStore, IUserStore authStore, IUnitOfWork unitOfWork, ITokenProvider tokenProvider) : IRequestHandler<RefreshTokenCommand, TokenModel>
{
    public async Task<TokenModel> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        using (var transaction = await unitOfWork.BeginTrasnactionAsync())
        {
            try
            {
                var refreshTokenEntity = await refreshTokenStore.GetByToken(request.Token);
                if (refreshTokenEntity == null)
                {
                    throw new NotFoundException("Refresh token not found");
                }
                if (refreshTokenEntity.ExpiredAt < DateTime.UtcNow)
                {
                    throw new UnauthorizedException("Refresh token expired");
                }
                var authEntity = await authStore.GetById(refreshTokenEntity.UserId);
                Console.WriteLine(refreshTokenEntity.UserAgent);
                string accessToken = tokenProvider.GenerateAccessToken(refreshTokenEntity.UserId, refreshTokenEntity.UserAgent, authEntity.Role);
                string refreshToken = tokenProvider.GenerateRefreshToken();
                refreshTokenEntity.Token = refreshToken;

                //TODO: Remove magic number
                refreshTokenEntity.ExpiredAt = DateTime.UtcNow.AddDays(7);

                await refreshTokenStore.Update(refreshTokenEntity);

                await unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();

                return new TokenModel(accessToken, refreshToken);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
