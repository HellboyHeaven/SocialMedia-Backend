using Application;
using MessageBus.Abstractions;

public class MessageManager(IMessageBus messageBus) : IMessageManager
{
    private readonly TimeSpan _timeout = TimeSpan.FromSeconds(10);
    public Task<bool> GetUserId(Guid userId) => messageBus.RequestAsync<Guid, bool>("user.exists", userId, _timeout);
}
