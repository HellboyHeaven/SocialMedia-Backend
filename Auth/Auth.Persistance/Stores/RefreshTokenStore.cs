using Core;
using Microsoft.EntityFrameworkCore;
using Persistance;

public class RefreshTokenStore(AuthDbContext dbContext) : IRefreshTokenStore
{
    public async Task<Guid> Create(RefreshTokenEntity entity)
    {
        var exists = await GetByUserIdAndAgent(entity.UserId, entity.UserAgent);

        if (exists != null)
        {
            Console.WriteLine($"{exists.Id}, {exists.UserAgent}, {exists.Token}");
            exists.Token = entity.Token;
            await Update(exists);
            return exists.Id;
        }
        else
        {
            var entry = await dbContext.RefreshTokens.AddAsync(entity);
            return entry.Entity.Id;
        }
    }

    public async Task Delete(RefreshTokenEntity entity)
    {
        dbContext.RefreshTokens.Remove(entity);
        await dbContext.SaveChangesAsync();
    }

    public Task DeleteRange(List<RefreshTokenEntity> entities)
    {
        throw new NotImplementedException();
    }

    public Task<List<RefreshTokenEntity>> GetAllExpired()
    {
        throw new NotImplementedException();
    }
    public async Task<RefreshTokenEntity?> GetByToken(string token)
    {
        return await dbContext.RefreshTokens.SingleOrDefaultAsync(r => r.Token == token);
    }

    public async Task<RefreshTokenEntity?> GetByUserIdAndAgent(Guid userId, string userAgent)
    {
        return await dbContext.RefreshTokens.SingleOrDefaultAsync(r => r.UserId == userId && r.UserAgent == userAgent);
    }

    public async Task Update(RefreshTokenEntity item)
    {
        var entity = await dbContext.RefreshTokens.SingleOrDefaultAsync(r => r.Id == item.Id);
        if (entity == null) return;
        dbContext.Entry(entity).CurrentValues.SetValues(item);
    }
}
