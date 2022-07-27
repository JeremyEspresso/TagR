using TagR.Application.Extensions;
using TagR.Database.Extensions;
using Remora.Discord.Hosting.Extensions;
using TagR.Bot.Extensions;
using Remora.Discord.API.Abstractions.Gateway.Commands;
using Remora.Discord.Gateway;
using Microsoft.Extensions.DependencyInjection;
using TagR.Bot.Responders;
using Remora.Discord.Gateway.Extensions;
using Remora.Discord.Rest.Extensions;

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