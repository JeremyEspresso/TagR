using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.API.Objects;
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

    public Task<Result<IMessage>> CreateMessageAsync(Snowflake channelId, string content, Optional<IMessageReference> messageReference,  bool allowMentions, CancellationToken ct = default)
    {
        Optional<IAllowedMentions> allowedMentions = new();
        if (!allowMentions)
        {
            allowedMentions = new AllowedMentions(MentionRepliedUser: false);
        }

        return _restChannelAPI.CreateMessageAsync(channelId, content, messageReference: messageReference, allowedMentions: allowedMentions, ct: ct);
    }
}

