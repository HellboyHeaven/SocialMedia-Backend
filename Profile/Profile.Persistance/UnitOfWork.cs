using Microsoft.EntityFrameworkCore.Storage;
using Core;

namespace Persistance;

public class UnitOfWork(ProfileDbContext dbContext) : IUnitOfWork
{
    public Task<IDbContextTransaction> BeginTrasnactionAsync() => dbContext.Database.BeginTransactionAsync();
    public Task SaveChangesAsync() => dbContext.SaveChangesAsync();
}
