using Core;

public interface IPostStore
{
    Task<List<PostEntity>> GetAll(int page, int pageSize);
    Task<List<PostEntity>> GetAllByAuthorId(Guid authorId, int page, int pageSize);
    Task<PostEntity> GetById(Guid id);
    Task<Guid> Create(PostEntity post);
    Task<Guid> Update(PostEntity post);
    Task Delete(Guid id);
    Task<bool> Exists(Guid id);
}
