namespace Application;

public class ProfileModel
{
    public required string Username { get; set; }
    public string Nickname { get; set; } = string.Empty;
    public string? Avatar { get; set; }
}
