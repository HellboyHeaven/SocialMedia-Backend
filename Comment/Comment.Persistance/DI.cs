using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Application;
using Microsoft.EntityFrameworkCore;



namespace Persistance;

public static class DI
{
    public static IServiceCollection InjectPersistence(this IServiceCollection services, IConfiguration configuration, bool connect = true)
    {
        services.ConnectDb(configuration, connect);
        services.AddServices();
        services.AddStores();

        return services;
    }

    private static IServiceCollection ConnectDb(this IServiceCollection services, IConfiguration configuration, bool connect = true)
    {
        if (connect)
            services.AddDbContext<CommentDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("Database") ?? ""));

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }

    private static IServiceCollection AddStores(this IServiceCollection services)
    {
        services.AddScoped<ICommentStore, CommentStore>();
        return services;
    }


}
