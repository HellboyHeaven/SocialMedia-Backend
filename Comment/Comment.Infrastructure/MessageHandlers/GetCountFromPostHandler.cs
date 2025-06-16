using Application;
using KafkaMessageBus;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public class GetCountFromPostHandler : MessageHandlerService<Guid, int>
{
    public override string Topic { get => "post.comment.count"; }

    public override async Task<int> ProcessAsync(string key, Guid message, IServiceProvider serviceProvider)
    {
        var commentStore = serviceProvider.GetRequiredService<ICommentStore>();
        var result = await commentStore.GetCountByPostId(message);
        return result;
    }
}
