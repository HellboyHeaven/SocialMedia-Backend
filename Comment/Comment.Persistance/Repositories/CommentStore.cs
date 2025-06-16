using Core;
using Microsoft.EntityFrameworkCore;
using ServiceExeption.Exceptions;

namespace Persistance;

public class CommentStore(CommentDbContext dbContext) : ICommentStore
{
    public async Task<Guid> Create(CommentEntity entity)
    {
        await dbContext.Comments.AddAsync(entity);
        await dbContext.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<Guid> Update(CommentEntity comment)
    {
        comment.EditedAt = DateTime.UtcNow;
        dbContext.Comments.Update(comment);
        await dbContext.SaveChangesAsync();
        return comment.Id;
    }
    public async Task<CommentEntity> GetById(Guid id) => await dbContext.Comments.FindAsync(id);

    public Task<CommentEntity> GetByAuthorIdAndPostId(Guid authorId, Guid postId)
    {
        throw new NotImplementedException();
    }

    public Task<List<CommentEntity>> GetAllByPostId(Guid postId, int page, int pageSize) =>
        dbContext.Comments.Where(c => c.PostId == postId)
            .OrderBy(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    public Task<List<CommentEntity>> GetAllByAuthorId(Guid authorId, int page, int pageSize) =>
        dbContext.Comments.Where(c => c.AuthorId == authorId)
            .OrderBy(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

    public async Task Delete(Guid id)
    {
        var comment = await dbContext.Comments.FindAsync(id);
        if (comment == null)
        {
            throw new KeyNotFoundException($"Comment with id {id} not found");
        }
        dbContext.Comments.Remove(comment);
        await dbContext.SaveChangesAsync();
    }

    public async Task<bool> Exists(Guid id) => await dbContext.Comments.AnyAsync(p => p.Id == id);

    public async Task<int> GetCountByPostId(Guid postId)
    {
        return await dbContext.Comments.CountAsync(p => p.PostId == postId);
    }
}
