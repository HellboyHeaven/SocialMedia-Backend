using MediatR;
using Core;
using ServiceExeption.Exceptions;
using Crypt = BCrypt.Net.BCrypt;

namespace Application;

public class LoginUserHandler(IUserStore authStore, IRefreshTokenStore refreshTokenStore, IUnitOfWork unitOfWork, ITokenProvider tokenProvider) : IRequestHandler<LoginUserCommand, TokenModel>
{
    public async Task<TokenModel> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        using (var transaction = await unitOfWork.BeginTrasnactionAsync())
        {
            try
            {
                var authEntity = await authStore.GetByLogin(request.Login);

                if (authEntity == null)
                    throw new NotFoundException($"User not found (login : {request.Login})");

                if (!Crypt.Verify(request.Password, authEntity.PasswordHash))
                    throw new UnauthorizedException($"Incorrect password (logn : {request.Login})");

                string accessToken = tokenProvider.GenerateAccessToken(authEntity.Id, request.UserAgent, authEntity.Role);
                string refreshToken = tokenProvider.GenerateRefreshToken();
                //TODO: Add Store Token
                var regreshTokenEntity = new RefreshTokenEntity
                {
                    UserId = authEntity.Id,
                    UserAgent = request.UserAgent,
                    Token = refreshToken,
                    ExpiredAt = DateTime.UtcNow.AddHours(1)
                };

                await refreshTokenStore.Create(regreshTokenEntity);

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
