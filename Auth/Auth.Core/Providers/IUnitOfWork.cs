using Microsoft.EntityFrameworkCore.Storage;

namespace Core;

public interface IUnitOfWork
{
    public Task<IDbContextTransaction> BeginTrasnactionAsync();
    public Task SaveChangesAsync();
}
