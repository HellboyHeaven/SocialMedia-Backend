using Confluent.Kafka;
using Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DI
{
    public static IServiceCollection InjectInfrastrcutrue(this IServiceCollection services, IConfiguration config)
    {
        services = services.AddKafkaService(config);
        return services;
    }

    public static IServiceCollection AddKafkaService(this IServiceCollection services, IConfiguration config)
    {
        var kafkaConfig = config.GetSection("Kafka");
        var topic = "post-service";

        var producerConfig = new ProducerConfig
        {
            BootstrapServers = kafkaConfig["BootstrapServers"]
        };

        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = kafkaConfig["BootstrapServers"],
            GroupId = kafkaConfig["GroupId"],
            AutoOffsetReset = Enum.Parse<AutoOffsetReset>(kafkaConfig["AutoOffsetReset"])
        };

        services.AddSingleton<IKafkaProducer>(_ =>
        {
            var producer = new ProducerBuilder<string, string>(producerConfig).Build();
            return new KafkaProducer(producer, topic);
        });

        services.AddSingleton<IKafkaConsumer>(_ =>
        {
            var consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
            return new KafkaConsumer(consumer);
        });

        return services;
    }
}
