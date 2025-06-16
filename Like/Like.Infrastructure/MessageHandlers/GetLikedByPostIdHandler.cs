using Application;
using Core;
using KafkaMessageBus;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public class GetLikedByPostIdHandler : MessageHandlerService<PostIdAndUserIdMessage, bool>
{
    public override string Topic { get => "post.like.exists"; }

    public async override Task<bool> ProcessAsync(string key, PostIdAndUserIdMessage message, IServiceProvider serviceProvider)
    {
        var likeStore = serviceProvider.GetRequiredService<ILikeStore>();
        var result = await likeStore.Exists<PostLike>(message.UserId, message.PostId);
        return result;
    }
}
