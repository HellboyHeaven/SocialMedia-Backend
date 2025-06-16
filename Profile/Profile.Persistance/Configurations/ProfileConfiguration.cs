using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ProfileConfiguration : IEntityTypeConfiguration<ProfileEntity>
{
    public void Configure(EntityTypeBuilder<ProfileEntity> builder)
    {
        builder.HasKey(e => e.UserId);
        builder.HasIndex(e => e.Username).IsUnique();

        var post = new ProfileEntity
        {
            UserId = Guid.Parse("a1234567-89ab-4cde-9012-3456789abcde"),
            Username = "admin"
        };

        builder.HasData(post);
    }
}
