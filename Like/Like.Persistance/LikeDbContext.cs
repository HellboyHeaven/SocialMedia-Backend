using Core;
using Microsoft.EntityFrameworkCore;

namespace Persistance;

public class LikeDbContext(DbContextOptions<LikeDbContext> options)
    : DbContext(options)
{
    public DbSet<LikeEntity> Likes { get; set; }
    public DbSet<PostLike> PostLikes { get; set; }
    public DbSet<CommentLike> CommentLikes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LikeEntity>().UseTpcMappingStrategy();
        modelBuilder.Entity<PostLike>().ToTable("PostLikes");
        modelBuilder.Entity<CommentLike>().ToTable("CommentLikes");

        modelBuilder.ApplyConfiguration(new PostLikeConfiguration());
        modelBuilder.ApplyConfiguration(new CommentLikeConfiguration());
    }
}
