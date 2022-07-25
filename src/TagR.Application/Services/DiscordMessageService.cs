using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Rest.Core;
using Remora.Results;
using TagR.Application.Services.Abstractions;

namespace TagR.Application.Services;

public class DiscordMessageService : IDiscordMessageService
{
    private readonly IDiscordRestChannelAPI _restChannelAPI;

    public DiscordMessageService(IDiscordRestChannelAPI restChannelAPI)
    {
        _restChannelAPI = restChannelAPI;
    }

    public Task<Result<IMessage>> CreateMessageAsync(Snowflake channelId, string content, CancellationToken ct = default)
    {
        return _restChannelAPI.CreateMessageAsync(channelId, content, ct: ct);
    }
}

