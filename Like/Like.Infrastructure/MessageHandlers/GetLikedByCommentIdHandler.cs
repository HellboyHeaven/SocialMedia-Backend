using Application;
using Core;
using KafkaMessageBus;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public class GetLikedByCommentIdHandler : MessageHandlerService<CommentIdAndUserIdMessage, bool>
{
    public override string Topic { get => "comment.like.exists"; }

    public async override Task<bool> ProcessAsync(string key, CommentIdAndUserIdMessage message, IServiceProvider serviceProvider)
    {
        var likeStore = serviceProvider.GetRequiredService<ILikeStore>();
        var result = await likeStore.Exists<CommentLike>(message.UserId, message.CommentId);
        return result;
    }
}
