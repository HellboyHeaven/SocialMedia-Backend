using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CommentLikeConfiguration : IEntityTypeConfiguration<CommentLike>
{
    public void Configure(EntityTypeBuilder<CommentLike> builder)
    {
        builder.Property(d => d.AuthorId).IsRequired();
        builder.Property(d => d.CommentId).IsRequired();
        builder.HasBaseType<LikeEntity>();
        builder.HasIndex(e => new { e.CommentId, e.AuthorId })
            .IsUnique()
            .HasDatabaseName("IX_CommentLike_CommentId_AuthorId");
        var entity = new CommentLike()
        {
            Id = Guid.Parse("6186e3f3-4b71-4555-9aeb-69c0c64a27c8"),
            AuthorId = Guid.Parse("a1234567-89ab-4cde-9012-3456789abcde"),
            CommentId = Guid.Parse("75c9cdb1-8706-4207-b0ee-792349916511"),
            CreatedAt = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc)
        };
        builder.HasData(entity);
    }
}
