using Application;
using KafkaMessageBus;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public class ExistsHandler : MessageHandlerService<Guid, bool>
{
    public override string Topic { get => "profile.exists"; }

    public async override Task<bool> ProcessAsync(string key, Guid message, IServiceProvider serviceProvider)
    {
        var postStore = serviceProvider.GetRequiredService<IProfileStore>();
        var result = await postStore.Exists(message);
        return result;
    }
}
