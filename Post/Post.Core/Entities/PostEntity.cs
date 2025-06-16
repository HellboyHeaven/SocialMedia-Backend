// using System.ComponentModel.DataAnnotations;
namespace Core;

public class PostEntity
{
    public Guid Id { get; set; }
    public required Guid AuthorId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string[] Medias { get; set; } = [];
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? EditedAt { get; set; } = null;
}
