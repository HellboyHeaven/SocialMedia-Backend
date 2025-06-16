using System;

namespace Core;

public interface IMessageConsumer
{
    Task<T?> ConsumeMessageAsync<T>(string topic, Guid key, CancellationToken cancellationToken, TimeSpan? timeout = null);
}
