using Microsoft.EntityFrameworkCore;

namespace Core;

[Index(nameof(AuthorId), nameof(PostId), IsUnique = true)]
public class PostLike : LikeEntity, ITargetable
{
    public required Guid PostId { get; set; }

    public Guid TargetId
    {
        get => PostId;
        set => PostId = value;
    }
}
