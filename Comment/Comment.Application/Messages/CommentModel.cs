namespace Application;

public class CommentModel
{
    public required Guid Id { get; set; }
    public required ProfileModel Author { get; set; }
    public string Content { get; set; } = "";
    public IEnumerable<string> Medias { get; set; } = [];
    public required DateTime CreatedAt { get; set; }
    public DateTime? EditedAt { get; set; } = null;
    public int Likes { get; set; } = 0;
    public bool Liked { get; set; } = false;
}
