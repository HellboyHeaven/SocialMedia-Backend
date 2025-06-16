namespace Core;

public interface IRefreshTokenStore
{
    public Task<Guid> Create(RefreshTokenEntity entity);
    public Task<RefreshTokenEntity?> GetByUserIdAndAgent(Guid userId, string userAgent);
    public Task<RefreshTokenEntity?> GetByToken(string token);
    public Task Update(RefreshTokenEntity entity);
    public Task Delete(RefreshTokenEntity entity);
    public Task<List<RefreshTokenEntity>> GetAllExpired();
    public Task DeleteRange(List<RefreshTokenEntity> entities);
}
