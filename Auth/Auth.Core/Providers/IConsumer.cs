namespace Core;

public interface IMessageBrokerConsumer
{
    public Task<T?> ConsumeMessageAsync<T>(string topic, string key, CancellationToken cancellationToken);
}
