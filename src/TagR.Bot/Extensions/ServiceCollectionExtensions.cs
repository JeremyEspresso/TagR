using Remora.Commands.Extensions;
using Remora.Discord.Commands.Extensions;
using Remora.Discord.Commands.Responders;
using Remora.Discord.Gateway.Extensions;
using TagR.Bot.Commands.Text;
using TagR.Bot.Responders;

namespace TagR.Bot.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddResponders(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddResponder<ReadyResponder>();
        serviceCollection.AddResponder<MessageResponder>();

        return serviceCollection;
    }

    public static IServiceCollection AddCommandGroups(this IServiceCollection serviceCollection, IConfig config)
    {
        serviceCollection.AddDiscordCommands();
        serviceCollection.Configure<CommandResponderOptions>(opt =>
        {
            opt.Prefix = config.Discord.CommandPrefix;
        });
        serviceCollection.AddCommandTree()
            .WithCommandGroup<TagCommandGroup>()
            .Finish();

        return serviceCollection;
    }
}

