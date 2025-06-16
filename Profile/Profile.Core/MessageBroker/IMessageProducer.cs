public interface IMessageProducer
{
    Task SendMessageAsync(string action, Guid key, string message);
    Task SendMessageAsync<T>(string action, Guid key, T obj);
}
