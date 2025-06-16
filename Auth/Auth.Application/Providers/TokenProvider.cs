using Core;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

public class TokenProvider : ITokenProvider
{
    // Статическое поле для хранения RSA ключа
    private static RSA? _rsa;
    private static readonly object _lock = new object();

    public string GenerateAccessToken(Guid userId, string userAgent, UserRole role)
    {
        var privateKeyPath = Environment.GetEnvironmentVariable("JWT_PRIVATE_KEY_PATH");
        if (string.IsNullOrEmpty(privateKeyPath) || !File.Exists(privateKeyPath))
            throw new InvalidOperationException("Private key file not found or JWT_PRIVATE_KEY_PATH not set.");

        var rsaKey = GetOrCreateRsaKey(privateKeyPath, isPrivateKey: true);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            new Claim(ClaimTypes.Role, role.ToString()),
            // Добавляем custom claims если нужно
            new Claim("user_agent", userAgent ?? "unknown")
        };

        var creds = new SigningCredentials(rsaKey, SecurityAlgorithms.RsaSha256);

        var token = new JwtSecurityToken(
            issuer: "yourdomain.com",
            audience: "yourdomain.com",
            claims: claims,
            notBefore: DateTime.UtcNow, // Добавляем notBefore
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public bool Validate()
    {
        // Реализация валидации токена
        throw new NotImplementedException();
    }

    private static RsaSecurityKey GetOrCreateRsaKey(string keyPath, bool isPrivateKey)
    {
        lock (_lock)
        {
            if (_rsa == null)
            {
                _rsa = RSA.Create();
                var keyText = File.ReadAllText(keyPath);

                try
                {
                    _rsa.ImportFromPem(keyText.ToCharArray());
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to import RSA key: {ex.Message}");
                }
            }

            return new RsaSecurityKey(_rsa);
        }
    }

    // Освобождение ресурсов
    public static void Dispose()
    {
        lock (_lock)
        {
            _rsa?.Dispose();
            _rsa = null;
        }
    }
}
