namespace Application;

public class BriefProfileModel
{
    public required string Username { get; set; }
    public string Nickname { get; set; } = string.Empty;
    public string? Avatar { get; set; }
    public bool IsAdmin { get; set; } = false;
}
