namespace Core;

public interface ITokenProvider
{
    public bool Validate();
    public string GenerateAccessToken(Guid userId, string userAgent, UserRole role);
    public string GenerateRefreshToken();
}
