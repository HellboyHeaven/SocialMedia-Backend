using Application;
using Core;
using Microsoft.EntityFrameworkCore;
using ServiceExeption.Exceptions;

namespace Persistance;


public class PostStore(PostDbContext dbContext) : IPostStore
{
    public async Task<List<PostEntity>> GetAll(int page, int pageSize)
    {
        return await dbContext.Posts
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public Task<List<PostEntity>> GetAllByAuthorId(Guid authorId, int page, int pageSize)
    {
        return dbContext.Posts
            .Where(p => p.AuthorId == authorId)
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<PostEntity> GetById(Guid id) =>
        await dbContext.Posts.SingleOrDefaultAsync(p => p.Id == id) ?? throw new NotFoundException("not Found");
    public async Task<Guid> Create(PostEntity post)
    {
        await dbContext.Posts.AddAsync(post);
        await dbContext.SaveChangesAsync();
        return post.Id;
    }

    public async Task<Guid> Update(PostEntity post)
    {
        post.EditedAt = DateTime.UtcNow;
        dbContext.Posts.Update(post);
        await dbContext.SaveChangesAsync();
        return post.Id;
    }

    public async Task Delete(Guid id)
    {
        var comment = await dbContext.Posts.FindAsync(id);
        if (comment == null)
        {
            throw new KeyNotFoundException($"Comment with id {id} not found");
        }
        dbContext.Posts.Remove(comment);
        await dbContext.SaveChangesAsync();
    }
    public async Task<bool> Exists(Guid id) => await dbContext.Posts.AsNoTracking().AnyAsync(p => p.Id == id);
}
