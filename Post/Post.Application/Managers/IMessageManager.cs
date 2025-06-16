namespace Application;

public interface IMessageManager
{
    Task<Guid?> GetUserIdAsync(string username, CancellationToken cancellationToken = default);
    Task<ProfileModel?> GetProfileAsync(Guid id, CancellationToken cancellationToken = default);
    Task<int> GetCommentCountAsync(Guid postId, CancellationToken cancellationToken = default);
    Task<int> GetLikeCountAsync(Guid postId, CancellationToken cancellationToken = default);
    Task<bool> LikeExistsAsync(Guid postId, Guid userId, CancellationToken cancellationToken = default);
}
