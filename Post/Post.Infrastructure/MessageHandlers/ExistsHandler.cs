using Application;
using KafkaMessageBus;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public class ExistsHandler : MessageHandlerService<Guid, bool>
{
    public override string Topic { get => "post.exists"; }
    public async override Task<bool> ProcessAsync(string key, Guid message, IServiceProvider serviceProvider)
    {
        Console.WriteLine($"Processing exists request for post {message}");
        var postStore = serviceProvider.GetRequiredService<IPostStore>();
        return await postStore.Exists(message);
    }
}


public class GetHandler : MessageHandlerService<PostIdAndUserIdMessage, PostModel>
{
    public override string Topic { get => "post.get"; }
    public async override Task<PostModel?> ProcessAsync(string key, PostIdAndUserIdMessage message, IServiceProvider serviceProvider)
    {
        var postStore = serviceProvider.GetRequiredService<IPostStore>();
        var messageManager = serviceProvider.GetRequiredService<IMessageManager>();

        var entity = await postStore.GetById(message.PostId);
        if (entity == null) return null;

        var profileTask = messageManager.GetProfileAsync(entity.AuthorId);
        var commentsTask = messageManager.GetCommentCountAsync(entity.Id);
        var likesTask = messageManager.GetLikeCountAsync(entity.Id);
        var likedTask = message.UserId == null ? Task.FromResult(false) : messageManager.LikeExistsAsync(entity.Id, message.UserId!.Value);
        await Task.WhenAll(profileTask, commentsTask, likesTask);

        var model = new PostModel
        {
            Id = entity.Id,
            Author = await profileTask,
            Content = entity.Content,
            Medias = entity.Medias,
            CreatedAt = entity.CreatedAt,
            EditedAt = entity.EditedAt,
            Comments = await commentsTask,
            Likes = await likesTask,
            Liked = await likedTask,
        };

        return model;
    }
}
