using Core;

public interface ICommentStore
{
    Task<CommentEntity> GetById(Guid id);
    Task<CommentEntity> GetByAuthorIdAndPostId(Guid authorId, Guid postId);
    Task<List<CommentEntity>> GetAllByPostId(Guid postId, int page, int pageSize);
    Task<List<CommentEntity>> GetAllByAuthorId(Guid authorId, int page, int pageSize);
    Task<Guid> Create(CommentEntity entity);
    Task<Guid> Update(CommentEntity entity);
    Task Delete(Guid id);
    Task<bool> Exists(Guid id);
    Task<int> GetCountByPostId(Guid postId);
}
