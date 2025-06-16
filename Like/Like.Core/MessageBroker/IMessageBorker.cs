using System;

namespace Core;

public interface IMessageBroker
{
    Task<R?> ProcessAsync<T, R>(string topic, Guid key, T obj, CancellationToken cancellationToken = default);
    Task<R?> ProcessAsync<R>(string topic, Guid key, string message, CancellationToken cancellationToken = default);
    Task ResponseAsync<T>(string topic, Guid key, T obj, CancellationToken cancellationToken = default);
    Task ResponseAsync(string topic, Guid key, string message, CancellationToken cancellationToken = default);
}
