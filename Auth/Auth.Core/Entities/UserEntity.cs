using System.ComponentModel.DataAnnotations.Schema;

namespace Core;

[Table("User")]
public class UserEntity
{
    public Guid Id { get; set; }
    public required string Login { get; set; }
    public required string PasswordHash { get; set; }
    public UserRole Role { get; set; } = UserRole.User;
}
