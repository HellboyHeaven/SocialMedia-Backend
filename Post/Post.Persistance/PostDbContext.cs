using Core;
using Microsoft.EntityFrameworkCore;

namespace Persistance;

public class PostDbContext(DbContextOptions<PostDbContext> options)
    : DbContext(options)
{
    public DbSet<PostEntity> Posts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PostConfiguration());
    }
}
