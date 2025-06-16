public class CreateProfileRequest
{
    public required string Username { get; set; }
    public string? Nickname { get; set; }
    public string Description { get; set; } = string.Empty;
    public IFormFile? Avatar { get; set; }
}
