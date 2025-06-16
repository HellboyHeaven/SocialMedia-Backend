using Application;
using MessageBus.Abstractions;

public class MessageManager(IMessageBus messageBus) : IMessageManager
{
    TimeSpan _timeout = TimeSpan.FromSeconds(10);

    public async Task<int> GetLikeCount(Guid id)
    {
        var response = await messageBus.RequestAsync<Guid, int?>("comment.like.count", id, _timeout);
        return response ?? 0;
    }

    public async Task<bool> GetLikeExists(Guid id, Guid userId)
    {
        var response = await messageBus.RequestAsync<CommentIdAndUserIdMessage, bool?>("comment.like.exists", new(id, userId), _timeout);
        return response ?? false;
    }

    public Task<PostModel?> GetPostById(Guid postId, Guid? userId)
    {
        return messageBus.RequestAsync<PostIdAndUserIdMessage, PostModel?>("post.get", new(postId, userId), _timeout);
    }

    public async Task<bool> GetPostExists(Guid postId)
    {
        var response = await messageBus.RequestAsync<Guid, bool?>("post.exists", postId, _timeout);
        return response ?? false;
    }


    public async Task<ProfileModel?> GetProfileById(Guid id)
    {
        var response = await messageBus.RequestAsync<Guid, ProfileModel>("profile.brief.get", id, _timeout);
        return response ?? null;
    }

    public async Task<bool> GetProfileExists(Guid userId)
    {
        var response = await messageBus.RequestAsync<Guid, bool?>("profile.exists", userId, _timeout);
        return response ?? false;
    }

    public Task<Guid?> GetUserIdAsync(string username)
    {
        return messageBus.RequestAsync<string, Guid?>("profile.userId.get-by-username", username, _timeout);
    }
}
