using TagR.Application.Extensions;
using TagR.Database.Extensions;
using Remora.Discord.Hosting.Extensions;
using TagR.Bot.Extensions;
using Remora.Discord.API.Abstractions.Gateway.Commands;
using Remora.Discord.Gateway;

var config = Config.Get();
var hostBuilder = Host.CreateDefaultBuilder(args);

hostBuilder
    .AddDiscordService(_ => config.Discord.Token)
    .ConfigureServices(
    (_, services) =>
    {
                services.AddSingleton(config);
                services.Configure<DiscordGatewayClientOptions>(g => g.Intents |= GatewayIntents.MessageContents);
                services.AddResponders();
                services.AddCommandGroups(config);
                services.AddApplication(config);
                services.AddDatabase(config);
    });

var host = hostBuilder.Build();

host.EnsureMigrated();

await host.RunAsync();