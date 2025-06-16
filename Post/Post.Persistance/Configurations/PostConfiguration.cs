using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PostConfiguration : IEntityTypeConfiguration<PostEntity>
{
    public void Configure(EntityTypeBuilder<PostEntity> builder)
    {
        var post = new PostEntity
        {
            Id = Guid.Parse("4e0e9111-9e6b-4e20-8b40-b5fd06fb9069"),
            AuthorId = Guid.Parse("a1234567-89ab-4cde-9012-3456789abcde"),
            Medias = ["https://blog.ishosting.com/hubfs/blog/what-is-url/url.png"],
            CreatedAt = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc)
        };

        builder.HasData(post);
    }
}
