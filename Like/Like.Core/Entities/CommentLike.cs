using Microsoft.EntityFrameworkCore;

namespace Core;

[Index(nameof(AuthorId), nameof(CommentId), IsUnique = true)]
public class CommentLike : LikeEntity, ITargetable
{
    public required Guid CommentId { get; set; }

    public Guid TargetId
    {
        get => CommentId;
        set => CommentId = value;
    }
}
