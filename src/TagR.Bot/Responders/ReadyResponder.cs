using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Gateway.Commands;
using Remora.Discord.Gateway;
using Remora.Discord.Gateway.Responders;
using Remora.Results;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Objects;
using Remora.Discord.API.Abstractions.Gateway.Commands;

namespace TagR.Bot.Responders;

public class ReadyResponder : IResponder<IReady>
{
    private readonly DiscordGatewayClient _gatewayClient;
    private readonly ILogger<ReadyResponder> _logger;

    public ReadyResponder
    (
        DiscordGatewayClient gatewayClient, 
        ILogger<ReadyResponder> logger
    )
    {
        _gatewayClient = gatewayClient;
        _logger = logger;
    }

    public Task<Result> RespondAsync
    (
        IReady gatewayEvent, 
        CancellationToken ct = default
    )
    {
        var currentUserId = gatewayEvent.User.ID.Value;

        _logger.LogInformation("Ready received - {currentUserId}", currentUserId);

        _gatewayClient.SubmitCommand(GetUpdatePresenceCommand());

        return Task.FromResult(Result.FromSuccess());
    }


    private IUpdatePresence GetUpdatePresenceCommand() =>
        new UpdatePresence
            (
                ClientStatus.Online,
                false,
                default,
                new IActivity[]
                {
                    new Activity("with tags", ActivityType.Game)
                }
            );
}

