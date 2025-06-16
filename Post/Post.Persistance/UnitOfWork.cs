using Microsoft.EntityFrameworkCore.Storage;
using Application;

namespace Persistance;

public class UnitOfWork(PostDbContext dbContext) : IUnitOfWork
{
    public Task<IDbContextTransaction> BeginTrasnactionAsync() => dbContext.Database.BeginTransactionAsync();
    public Task SaveChangesAsync() => dbContext.SaveChangesAsync();
}
