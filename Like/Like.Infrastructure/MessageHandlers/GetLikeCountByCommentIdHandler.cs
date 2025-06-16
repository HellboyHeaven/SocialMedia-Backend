using Core;
using KafkaMessageBus;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public class GetLikeCountByCommentIdHandler : MessageHandlerService<Guid, int>
{
    public override string Topic { get => "comment.like.count"; }

    public async override Task<int> ProcessAsync(string key, Guid message, IServiceProvider serviceProvider)
    {
        var likeStore = serviceProvider.GetRequiredService<ILikeStore>();
        var response = await likeStore.GetLikesCount<CommentLike>(message);
        return response;
    }
}
