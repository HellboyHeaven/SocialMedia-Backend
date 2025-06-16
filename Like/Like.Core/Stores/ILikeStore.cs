using Core;

namespace Core;

public interface ILikeStore
{
    Task<T> GetLike<T>(Guid authorId, Guid targetId) where T : LikeEntity, ITargetable;
    Task Create(LikeEntity like);
    Task Delete<T>(Guid authorId, Guid targetId) where T : LikeEntity, ITargetable;
    Task<bool> Exists<T>(Guid authorId, Guid targetId) where T : LikeEntity, ITargetable;
    Task<int> GetLikesCount<T>(Guid targetId) where T : LikeEntity, ITargetable;
}
