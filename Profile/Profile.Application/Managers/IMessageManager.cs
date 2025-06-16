namespace Application;

public interface IMessageManager
{
    public Task<bool> GetUserId(Guid userId);
}
