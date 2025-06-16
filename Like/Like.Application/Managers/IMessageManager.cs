namespace Application;

public interface IMessageManager
{
    public Task<bool> PostExists(Guid postId);
    public Task<bool> CommentExists(Guid commentId);
    public Task<bool> ProfileExists(Guid userId);
}
