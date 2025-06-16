namespace Core;

public interface IUserStore
{
    public Task<Guid> Create(UserEntity userEntity);
    public Task<UserEntity> GetById(Guid id);
    public Task<UserEntity> GetByLogin(string login);
    public Task ChangePassword(string login, string password);
}
