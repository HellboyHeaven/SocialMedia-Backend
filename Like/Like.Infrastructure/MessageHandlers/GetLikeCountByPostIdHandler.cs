using Core;
using KafkaMessageBus;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public class GetLikeCountByPostIdHandler : MessageHandlerService<Guid, int>
{
    public override string Topic { get => "post.like.count"; }

    public async override Task<int> ProcessAsync(string key, Guid message, IServiceProvider serviceProvider)
    {
        var likeStore = serviceProvider.GetRequiredService<ILikeStore>();
        var result = await likeStore.GetLikesCount<PostLike>(message);
        return result;
    }
}
