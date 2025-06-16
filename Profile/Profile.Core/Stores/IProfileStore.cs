using Core;

public interface IProfileStore
{
    Task<ProfileEntity> GetByUsername(string username);
    Task<ProfileEntity> GetByUserId(Guid userId);
    Task<Guid> Create(ProfileEntity post);
    Task<Guid> Update(ProfileEntity post);
    Task<bool> Exists(Guid userId);
    Task DeleteByUsername(string username);
}
