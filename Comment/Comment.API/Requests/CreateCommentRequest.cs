public class CreateCommentRequest
{
    public required Guid PostId { get; set; }
    public string Content { get; set; } = string.Empty;
    public IFormFile[] Medias { get; set; } = [];
}
