using Microsoft.EntityFrameworkCore;
using Core;
using ServiceExeption.Exceptions;

namespace Persistance;

public class UserStore(AuthDbContext dbContext) : IUserStore
{
    public async Task<Guid> Create(UserEntity authEntity)
    {
        var exists = await dbContext.Users.AnyAsync(s => s.Login == authEntity.Login);
        if (exists)
            throw new ConflictException($"User already exist (login : {authEntity.Login})");

        var entry = await dbContext.Users.AddAsync(authEntity);
        return entry.Entity.Id;
    }

    public async Task<UserEntity> GetById(Guid id)
        => await dbContext.Users.SingleOrDefaultAsync(s => s.Id == id) ?? throw new NotFoundException($"User not found (id : {id})");

    public async Task<UserEntity> GetByLogin(string login)
        => await dbContext.Users.SingleOrDefaultAsync(s => s.Login == login) ?? throw new NotFoundException($"User not found (login : {login})");

    public async Task ChangePassword(string login, string passwordHash)
    {
        var user = await dbContext.Users.SingleOrDefaultAsync(s => s.Login == login) ?? throw new NotFoundException($"User not found (login : {login})");
        user.PasswordHash = passwordHash;
    }
}
