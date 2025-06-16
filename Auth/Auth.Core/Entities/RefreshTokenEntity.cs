using System.ComponentModel.DataAnnotations.Schema;

namespace Core;

[Table("RefreshToken")]
public class RefreshTokenEntity
{
    public Guid Id { get; set; }
    public required Guid UserId { get; set; }
    public UserEntity User { get; set; }
    public required string UserAgent { get; set; }
    public required string Token { get; set; }
    public required DateTime ExpiredAt { get; set; }
}
