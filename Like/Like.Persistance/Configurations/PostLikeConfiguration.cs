using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PostLikeConfiguration : IEntityTypeConfiguration<PostLike>
{
    public void Configure(EntityTypeBuilder<PostLike> builder)
    {
        builder.Property(d => d.AuthorId).IsRequired();
        builder.Property(d => d.PostId).IsRequired();
        builder.HasBaseType<LikeEntity>();
        builder.HasIndex(e => new { e.PostId, e.AuthorId })
            .IsUnique()
            .HasDatabaseName("IX_PostLike_PostId_AuthorId");
        var entity = new PostLike()
        {
            Id = Guid.Parse("c7e7774a-ff56-4de5-83ae-9fc5742ca05b"),
            AuthorId = Guid.Parse("a1234567-89ab-4cde-9012-3456789abcde"),
            PostId = Guid.Parse("4e0e9111-9e6b-4e20-8b40-b5fd06fb9069"),
            CreatedAt = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc)
        };

        builder.HasData(entity);
    }
}
