using Application;
using KafkaMessageBus;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public class GetIdByUsernameHandler : MessageHandlerService<string, Guid?>
{
    public override string Topic { get => "profile.userId.get-by-username"; }

    public async override Task<Guid?> ProcessAsync(string key, string message, IServiceProvider serviceProvider)
    {
        var profileStore = serviceProvider.GetRequiredService<IProfileStore>();
        var profile = await profileStore.GetByUsername(message);
        return profile?.UserId ?? null;
    }
}
