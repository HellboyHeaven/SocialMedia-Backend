using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CommentConfiguration : IEntityTypeConfiguration<CommentEntity>
{
    public void Configure(EntityTypeBuilder<CommentEntity> builder)
    {
        var entity = new CommentEntity
        {
            Id = Guid.Parse("75c9cdb1-8706-4207-b0ee-792349916511"),
            AuthorId = Guid.Parse("a1234567-89ab-4cde-9012-3456789abcde"),
            PostId = Guid.Parse("4e0e9111-9e6b-4e20-8b40-b5fd06fb9069"),
            Content = "This is a sample comment.",
            CreatedAt = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc),
        };

        builder.HasData(entity);
    }
}
