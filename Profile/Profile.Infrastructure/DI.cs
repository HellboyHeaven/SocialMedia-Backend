using Application;
using System.Reflection;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using KafkaMessageBus.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DI
{
    public static IServiceCollection InjectInfrastrcutrue(this IServiceCollection services, IConfiguration config)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddCdn(config);
        services.AddScoped<IMessageManager, MessageManager>();
        services.AddMessageBus(config, "Kafka", Assembly.GetExecutingAssembly());
        return services;
    }


    private static IServiceCollection AddCdn(this IServiceCollection services, IConfiguration configuration)
    {
        if (EF.IsDesignTime)
        {
            Console.WriteLine("Skipping AWS CDN configuration for EF design-time");
            return services;
        }

        var accessKey = configuration["AWS:AccessKey"];
        var secretKey = configuration["AWS:SecretKey"];
        var region = configuration["AWS:Region"];

        if (string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(region))
        {
            Console.WriteLine("AWS configuration incomplete, skipping CDN setup");
            return services;
        }

        try
        {
            var credentials = new BasicAWSCredentials(accessKey, secretKey);
            var s3Config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName(region)
            };
            var s3Client = new AmazonS3Client(credentials, s3Config);
            services.AddSingleton<IAmazonS3>(s3Client);
            services.AddSingleton<ICdnProvider, CloudFrontCdnProvider>();

            Console.WriteLine($"AWS S3 client configured for region: {region}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to configure AWS S3 client: {ex.Message}");
        }

        return services;
    }
}
