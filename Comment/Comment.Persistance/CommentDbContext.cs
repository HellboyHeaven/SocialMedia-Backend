using Core;
using Microsoft.EntityFrameworkCore;

namespace Persistance;

public class CommentDbContext(DbContextOptions<CommentDbContext> options)
    : DbContext(options)
{
    public DbSet<CommentEntity> Comments { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CommentConfiguration());
    }
}
