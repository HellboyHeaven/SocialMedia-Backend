using Core;
using Microsoft.EntityFrameworkCore;

namespace Persistance;

public class LikeStore(LikeDbContext context) : ILikeStore
{
    public async Task<T> GetLike<T>(Guid authorId, Guid targetId) where T : LikeEntity, ITargetable
    {
        return await context.Set<T>().FirstOrDefaultAsync(l => l.AuthorId == authorId && l.TargetId == targetId);
    }

    public async Task Create(LikeEntity like)
    {
        context.Entry(like).State = EntityState.Added;
        await context.SaveChangesAsync();
    }

    public async Task Delete<T>(Guid authorId, Guid targetId) where T : LikeEntity, ITargetable
    {
        var like = await GetLike<T>(authorId, targetId);
        if (like != null)
        {
            context.Set<T>().Remove(like);
            await context.SaveChangesAsync();
        }
    }

    public async Task<bool> Exists<T>(Guid authorId, Guid targetId) where T : LikeEntity, ITargetable
    {
        return await context.Set<T>()
            .AnyAsync(l => l.AuthorId == authorId && l.TargetId == targetId);
    }

    public async Task<int> GetLikesCount<T>(Guid targetId) where T : LikeEntity, ITargetable
    {
        return await context.Set<T>()
            .CountAsync(l => l.TargetId == targetId);
    }
}
