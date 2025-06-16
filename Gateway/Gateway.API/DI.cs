using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Yarp.ReverseProxy.Configuration;

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
        services.AddCustomReverseProxy(config);
        return services;
    }

    private static IServiceCollection AddCustomAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "yourdomain.com",
                    ValidAudience = "yourdomain.com",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_super_secret_key_which_is_at_least_32_bytes!"))
                };
            });
        services.AddAuthorization();

        return services;
    }



    private static IServiceCollection AddCustomReverseProxy(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IProxyConfigProvider>(new DynamicProxyConfigProvider(configuration)).AddReverseProxy();
        // IProxyConfigProvider a = new DynamicProxyConfigProvider(configuration);
        // services.AddReverseProxy().LoadFromMemory(a.GetConfig().Routes, a.GetConfig().Clusters);
        // services.AddReverseProxy()
        //     .LoadFromConfig(configuration.GetSection("ReverseProxy"));

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

            // Добавляем поддержку JWT в Swagger UI
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference= new OpenApiReference
                            {
                                Type= ReferenceType.SecurityScheme,
                                Id= "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
            });
        });

        return services;
    }


}
