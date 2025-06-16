using Application;
using MessageBus.Abstractions;

public class MessageManager(IMessageBus messageBus) : IMessageManager
{
    private readonly TimeSpan _timeout = TimeSpan.FromSeconds(10);
    public Task<bool> CommentExists(Guid commentId) => messageBus.RequestAsync<Guid, bool>("comment.exists", commentId, _timeout);
    public Task<bool> PostExists(Guid postId) => messageBus.RequestAsync<Guid, bool>("post.exists", postId, _timeout);
    public Task<bool> ProfileExists(Guid userId) => messageBus.RequestAsync<Guid, bool>("profile.exists", userId, _timeout);
}
