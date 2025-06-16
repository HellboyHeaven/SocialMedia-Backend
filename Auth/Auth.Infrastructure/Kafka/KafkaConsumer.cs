using Confluent.Kafka;
using Core;
using System.Text.Json;

namespace Infrastructure;

public class KafkaConsumer(IConsumer<string, string> consumer) : IMessageBrokerConsumer
{
    public async Task<T?> ConsumeMessageAsync<T>(string topic, string key, CancellationToken cancellationToken)
    {
        consumer.Subscribe(topic);
        while (!cancellationToken.IsCancellationRequested)
        {
            var consumeResult = consumer.Consume(cancellationToken);
            if (consumeResult.Message.Key == key)
                return JsonSerializer.Deserialize<T>(consumeResult.Message.Value);
            await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
        }
        consumer.Close();
        return default;
    }
}
