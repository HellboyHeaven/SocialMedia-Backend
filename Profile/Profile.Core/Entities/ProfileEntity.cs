using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Core;

[Index(nameof(Username), IsUnique = true)]
public class ProfileEntity
{
    [Key]
    public required Guid UserId { get; set; }
    [Required]
    public required string Username { get; set; }
    public string? Nickname { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? Avatar { get; set; }
}
