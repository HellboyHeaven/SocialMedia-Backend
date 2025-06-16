using Application;
using KafkaMessageBus;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public class ExistsHandler : MessageHandlerService<Guid, bool>
{
    public override string Topic { get => "comment.exists"; }

    public override async Task<bool> ProcessAsync(string key, Guid message, IServiceProvider serviceProvider)
    {
        var commentStore = serviceProvider.GetRequiredService<ICommentStore>();
        var result = await commentStore.Exists(message);
        return result;
    }
}
