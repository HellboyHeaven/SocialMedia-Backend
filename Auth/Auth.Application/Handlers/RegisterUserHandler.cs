using MediatR;
using Core;
using Crypt = BCrypt.Net.BCrypt;

namespace Application;

public class RegisterUserHandler(IUserStore authStore, IUnitOfWork unitOfWork) : IRequestHandler<RegisterUserCommand>
{
    public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        using (var transaction = await unitOfWork.BeginTrasnactionAsync())
        {
            try
            {
                var hash = Crypt.HashPassword(request.Password);
                var entity = new UserEntity { Login = request.Login, PasswordHash = hash };
                await authStore.Create(entity);
                await unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
