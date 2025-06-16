namespace Application;

public class CommentModel
{
    public required Guid Id { get; set; }
    public required BriefProfileModel Author { get; set; }
    public string Content { get; set; } = "";
    public IEnumerable<string> Medias { get; set; } = [];
    public required DateTime CreatedAt { get; set; }
}
