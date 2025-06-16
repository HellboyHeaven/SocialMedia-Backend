using Application;
using KafkaMessageBus;
using ServiceExeption.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public class GetByPostIdHandler : MessageHandlerService<Guid, BriefProfileModel?>
{
    public override string Topic { get => "profile.brief.get"; }

    public async override Task<BriefProfileModel?> ProcessAsync(string key, Guid message, IServiceProvider serviceProvider)
    {
        var profileStore = serviceProvider.GetRequiredService<IProfileStore>();
        var profile = await profileStore.GetByUserId(message);
        return new BriefProfileModel
        {
            Username = profile.Username,
            Nickname = profile.Nickname,
            Avatar = profile.Avatar
        } ?? null;
    }
}
