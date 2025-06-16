using Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Application;

public static class DI
{
    public static IServiceCollection InjectApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterMediatR();
        services.AddProviders();
        return services;
    }

    private static IServiceCollection RegisterMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
             cfg.RegisterServicesFromAssembly(typeof(DI).Assembly));
        return services;
    }

    private static IServiceCollection AddProviders(this IServiceCollection services)
    {
        services.AddScoped<ITokenProvider, TokenProvider>();
        return services;
    }
}
