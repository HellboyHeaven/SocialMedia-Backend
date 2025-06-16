using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Persistance;

namespace API;

public class ProfileDbContextFactory : IDesignTimeDbContextFactory<ProfileDbContext>
{
    public ProfileDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ProfileDbContext>();

        // Заглушка connection string для PostgreSQL (только для EF migrations)
        // В runtime используется connection string из Docker Compose
        optionsBuilder.UseNpgsql("Host=localhost;Database=TempMigrationsDb;Username=postgres;Password=postgres");

        return new ProfileDbContext(optionsBuilder.Options);
    }
}
