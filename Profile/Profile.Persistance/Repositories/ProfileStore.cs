using Core;
using Microsoft.EntityFrameworkCore;
using ServiceExeption.Exceptions;

namespace Persistance;

public class ProfileStore(ProfileDbContext dbContext) : IProfileStore
{
    public async Task<ProfileEntity> GetByUsername(string username) =>
        await dbContext.Profiles.FirstOrDefaultAsync(p => p.Username == username) ?? throw new NotFoundException($"Profile of {username} not found");

    public async Task<ProfileEntity> GetByUserId(Guid userId) =>
        await dbContext.Profiles.FirstOrDefaultAsync(p => p.UserId == userId) ?? throw new NotFoundException($"Profile of {userId} not found");

    public async Task<Guid> Create(ProfileEntity post)
    {
        await dbContext.Profiles.AddAsync(post);
        await dbContext.SaveChangesAsync();
        return post.UserId;
    }

    public Task<Guid> Update(ProfileEntity post)
    {
        throw new NotImplementedException();
    }

    public Task DeleteByUsername(string username)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> Exists(Guid userId) => await dbContext.Profiles.AnyAsync(p => p.UserId == userId);
}
