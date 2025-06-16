public class EditCommentRequest
{
    public string Content { get; set; }
    public string[] OldMedias { get; set; } = [];
    public IFormFile[] NewMedias { get; set; } = [];
}
