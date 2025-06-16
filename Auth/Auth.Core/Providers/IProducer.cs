namespace Core;

public interface IMessageBrokerProducer
{
    public Task SendMessageAsync(string action, string key, string message);

    public Task SendMessageAsync<T>(string action, string key, T obj);
}
