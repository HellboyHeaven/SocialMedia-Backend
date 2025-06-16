using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Core;
using Microsoft.EntityFrameworkCore;


namespace Persistance;

public static class DI
{
    public static IServiceCollection InjectPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConnectDb(configuration);
        services.AddServices();
        services.AddStores();
        return services;
    }

    private static IServiceCollection ConnectDb(this IServiceCollection services, IConfiguration configuration)
    {
        var con = configuration.GetConnectionString("Database");
        services.AddDbContext<AuthDbContext>(options =>
            options.UseNpgsql(con, o => o.MapEnum<UserRole>("role")));

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }

    private static IServiceCollection AddStores(this IServiceCollection services)
    {
        services.AddScoped<IUserStore, UserStore>();
        services.AddScoped<IRefreshTokenStore, RefreshTokenStore>();
        return services;
    }


}
