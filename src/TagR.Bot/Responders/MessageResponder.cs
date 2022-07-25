using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;
using TagR.Application.Services.Abstractions;

namespace TagR.Bot.Responders;

public class MessageResponder : IResponder<IMessageCreate>
{
    private readonly char _prefix;
    private readonly IMessageProcessingService _messageProcessing;

    public MessageResponder(IConfig config, IMessageProcessingService messageProcessing)
    {
        _prefix = config.Discord.CommandPrefix[0];
        _messageProcessing = messageProcessing;
    }

    public async Task<Result> RespondAsync(IMessageCreate gatewayEvent, CancellationToken ct = default)
    {
        if
        (
               (gatewayEvent.Author.IsBot.IsDefined(out var isBot) && isBot)
            || (gatewayEvent.Author.IsSystem.IsDefined(out var isSystem) && isSystem)
        )
            return Result.FromSuccess();

        var content = gatewayEvent.Content;
        var firstChar = content[0];

        if (!firstChar.Equals(_prefix))
            return Result.FromSuccess();

        await _messageProcessing.ProcessMessageAsync(gatewayEvent.ChannelID, content, ct);
        return Result.FromSuccess();
    }
}

