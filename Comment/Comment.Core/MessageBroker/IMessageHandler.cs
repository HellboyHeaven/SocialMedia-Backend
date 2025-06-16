namespace Core;

public interface IMessageHandler
{
    string Topic { get; }
    Task HandleAsync(string message, Guid key, CancellationToken cancellationToken);
}
