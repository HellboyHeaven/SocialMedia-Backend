using System.ComponentModel.DataAnnotations;

namespace Core;

public abstract class LikeEntity
{
    public Guid Id { get; set; }
    public required Guid AuthorId { get; set; }
    public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
