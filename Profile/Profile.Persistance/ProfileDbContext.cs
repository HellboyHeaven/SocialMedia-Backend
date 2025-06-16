using Core;
using Microsoft.EntityFrameworkCore;

namespace Persistance;

public class ProfileDbContext(DbContextOptions<ProfileDbContext> options)
    : DbContext(options)
{
    public DbSet<ProfileEntity> Profiles { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProfileConfiguration());
    }
}
