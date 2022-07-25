using TagR.Application.Services;
using TagR.Database;

namespace TagR.Application.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection, IConfig config)
    {
        serviceCollection.AddScoped<IMessageProcessingService, MessageProcessingService>();
        serviceCollection.AddSingleton<IDiscordMessageService, DiscordMessageService>();
        return serviceCollection;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection serviceCollection, IConfig config)
    {
        serviceCollection.AddNpgsql<TagRDbContext>
            (
                config.Database.ConnectionString,
                opt => opt.MigrationsAssembly(typeof(TagRDbContext).Assembly.FullName)
            );

        return serviceCollection;
    }
}
