using Application;
using MessageBus.Abstractions;

namespace Infrastructure;

public class MessageManager(IMessageBus messageBus) : IMessageManager
{
    private readonly TimeSpan _timeout = TimeSpan.FromSeconds(10);

    public async Task<int> GetCommentCountAsync(Guid postId, CancellationToken cancellationToken)
    {
        var response = await messageBus.RequestAsync<Guid, int?>("post.comment.count", postId, _timeout, cancellationToken);
        return response ?? 0;
    }

    public async Task<int> GetLikeCountAsync(Guid postId, CancellationToken cancellationToken)
    {
        var response = await messageBus.RequestAsync<Guid, int?>("post.like.count", postId, _timeout, cancellationToken);
        return response ?? 0;
    }

    public async Task<bool> LikeExistsAsync(Guid postId, Guid userId, CancellationToken cancellationToken)
    {
        var response = await messageBus.RequestAsync<PostIdAndUserIdMessage, bool?>("post.like.exists", new(postId, userId), _timeout, cancellationToken);
        return response ?? false;
    }

    public async Task<ProfileModel?> GetProfileAsync(Guid id, CancellationToken cancellationToken)
    {
        return await messageBus.RequestAsync<Guid, ProfileModel>("profile.brief.get", id, _timeout, cancellationToken) ?? null;
    }
    public Task<Guid?> GetUserIdAsync(string username, CancellationToken cancellationToken = default)
    {
        return messageBus.RequestAsync<string, Guid?>("profile.userId.get-by-username", username, _timeout, cancellationToken);
    }
}
