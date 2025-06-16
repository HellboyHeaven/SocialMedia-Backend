using Microsoft.EntityFrameworkCore.Storage;

namespace Application;

public interface IUnitOfWork
{
    public Task<IDbContextTransaction> BeginTrasnactionAsync();
    public Task SaveChangesAsync();
}
