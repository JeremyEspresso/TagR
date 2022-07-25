using Remora.Discord.Gateway.Extensions;
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
}

