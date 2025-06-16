using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Persistance;

namespace API;

public class LikeDbContextFactory : IDesignTimeDbContextFactory<LikeDbContext>
{
    public LikeDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<LikeDbContext>();

        // Заглушка connection string для PostgreSQL (только для EF migrations)
        // В runtime используется connection string из Docker Compose
        optionsBuilder.UseNpgsql("Host=localhost;Database=TempMigrationsDb;Username=postgres;Password=postgres");

        return new LikeDbContext(optionsBuilder.Options);
    }
}
