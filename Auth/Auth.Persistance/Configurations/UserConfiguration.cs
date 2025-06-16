using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Persistance;
public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        var admin = new UserEntity
        {
            Id = Guid.Parse("a1234567-89ab-4cde-9012-3456789abcde"),
            Login = "admin",
            PasswordHash = "$2a$10$4cOi70lokMvP3bIsu4cWweIMaq9O9C53UelbFm3HSOkIgxxxc.d4W",
            Role = UserRole.Admin
        };
        builder.HasData(admin);
    }
}
