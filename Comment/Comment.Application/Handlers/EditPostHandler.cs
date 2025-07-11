using MediatR;
using ServiceExeption.Exceptions;

namespace Application;

public class EditPostHandler(ICommentStore commentStore, ICdnProvider cdnProvider, IMessageManager messageManager, ICurrentUserService currentUser) : IRequestHandler<EditCommentCommand, CommentModel>
{
    public async Task<CommentModel> Handle(EditCommentCommand request, CancellationToken cancellationToken)
    {
        var entity = await commentStore.GetById(request.Id);

        if (entity == null)
            throw new NotFoundException("Post not found");

        if (entity.AuthorId != currentUser.Id)
            throw new UnauthorizedAccessException("You are not the author of this post");

        // Список медиа, которые остались после редактирования
        var remainingOldMedias = request.OldMedias ?? Array.Empty<string>();
        var currentMedias = entity.Medias ?? Array.Empty<string>();

        // Вычисляем какие старые медиа удалить (которые были, но их нет в оставшихся)
        var mediasToDelete = currentMedias.Except(remainingOldMedias).ToList();

        // Если контент и оставшиеся медиа не изменились и нет новых медиа - выбрасываем ошибку конфликта
        bool contentUnchanged = entity.Content == request.Content;
        bool mediasUnchanged = mediasToDelete.Count == 0 && (request.NewMedias.Length == 0);
        if (contentUnchanged && mediasUnchanged)
            throw new ConflictException("Post content and media are the same");

        // Удаляем удалённые медиа
        if (mediasToDelete.Count > 0)
            await cdnProvider.DeleteFilesAsync(mediasToDelete);

        // Загружаем новые медиа, если есть
        var newMediaUrls = new List<string>();
        if (request.NewMedias.Length > 0)
        {
            newMediaUrls = (await cdnProvider.UploadFilesAsync(request.NewMedias)).ToList();
        }

        // Обновляем сущность: объединяем оставшиеся старые медиа и новые загруженные
        entity.Medias = remainingOldMedias.Concat(newMediaUrls).ToArray();
        entity.Content = request.Content;

        await commentStore.Update(entity);

        var model = new CommentModel
        {
            Id = entity.Id,
            Author = await messageManager.GetProfileById(entity.AuthorId),
            Content = entity.Content,
            Medias = entity.Medias,
            CreatedAt = entity.CreatedAt,
            EditedAt = entity.EditedAt
        };

        return model;
    }
}
