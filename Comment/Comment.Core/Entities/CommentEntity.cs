using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Core;

public class CommentEntity
{
    public Guid Id { get; set; }
    public required Guid AuthorId { get; set; }
    public required Guid PostId { get; set; }
    public string Content { get; set; } = string.Empty;
    public IEnumerable<string> Medias { get; set; } = [];
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? EditedAt { get; set; } = null;
}
