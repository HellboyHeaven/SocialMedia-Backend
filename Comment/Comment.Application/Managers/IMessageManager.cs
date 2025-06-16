namespace Application;

public interface IMessageManager
{
    public Task<ProfileModel?> GetProfileById(Guid id);
    public Task<PostModel?> GetPostById(Guid postId, Guid? userId);
    public Task<bool> GetPostExists(Guid postId);
    public Task<bool> GetProfileExists(Guid userId);
    public Task<int> GetLikeCount(Guid id);
    public Task<bool> GetLikeExists(Guid id, Guid userId);
    public Task<Guid?> GetUserIdAsync(string username);
}
