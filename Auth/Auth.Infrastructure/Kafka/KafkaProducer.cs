using System.Text.Json;
using Confluent.Kafka;
using Core;

namespace Infrastructure;

public class KafkaProducer(IProducer<string, string> producer, string topic) : IMessageBrokerProducer
{
    public async Task SendMessageAsync(string action, string key, string message)
    {
        await producer.ProduceAsync($"{topic}/{action}", new Message<string, string> { Key = key, Value = message });
        producer.Flush();
    }

    public async Task SendMessageAsync<T>(string action, string key, T obj)
    {
        await SendMessageAsync(action, key, JsonSerializer.Serialize(obj));
    }
}
