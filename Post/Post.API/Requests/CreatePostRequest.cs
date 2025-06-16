public class CreatePostRequest
{
    public required string Content { get; set; }
    public List<IFormFile> Medias { get; set; } = [];
}
