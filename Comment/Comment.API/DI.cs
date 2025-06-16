using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Cryptography;
using Microsoft.OpenApi.Models;

namespace API;

public static class DI
{
    public static IServiceCollection InjectAPI(this IServiceCollection services, IConfiguration config)
    {
        services.AddControllers();
        services.AddOpenApi();
        services.AddEndpointsApiExplorer();
        services.AddCustomSwagger();
        services.AddCustomAuthentication();
        services.AddProblemDetails();
        return services;
    }

    private static IServiceCollection AddCustomAuthentication(this IServiceCollection services)
    {
        var publicKeyPath = Environment.GetEnvironmentVariable("JWT_PUBLIC_KEY_PATH");
        if (string.IsNullOrEmpty(publicKeyPath) || !File.Exists(publicKeyPath))
            throw new InvalidOperationException("Public key file not found or JWT_PUBLIC_KEY_PATH not set.");

        var publicKeyText = File.ReadAllText(publicKeyPath);

        // Создаем RSA ключ один раз и используем его
        var rsa = RSA.Create();
        try
        {
            rsa.ImportFromPem(publicKeyText.ToCharArray());
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to import public RSA key: {ex.Message}");
        }

        var rsaKey = new RsaSecurityKey(rsa);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false; // Для разработки, в продакшене должно быть true
            options.SaveToken = true;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.FromMinutes(1), // Уменьшаем допустимое расхождение времени

                ValidIssuer = "yourdomain.com",
                ValidAudience = "yourdomain.com",
                IssuerSigningKey = rsaKey,

                // Дополнительные параметры для отладки
                RequireExpirationTime = true,
                RequireSignedTokens = true
            };

            // Обработчики событий для отладки
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    Console.WriteLine("Token validated successfully");
                    return Task.CompletedTask;
                },
                OnMessageReceived = context =>
                {
                    var token = context.Token;
                    Console.WriteLine($"Token received: {token?[..Math.Min(50, token?.Length ?? 0)]}...");
                    return Task.CompletedTask;
                }
            };
        });

        return services;
    }

    private static IServiceCollection AddCustomSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "My API",
                Version = "v1",
                Description = "API documentation with Swagger"
            });

            // Исправленная настройка JWT для Swagger UI
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Enter your JWT token below (without 'Bearer' prefix).",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }


}
