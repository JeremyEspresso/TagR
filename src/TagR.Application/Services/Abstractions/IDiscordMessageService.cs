using Remora.Discord.API.Abstractions.Objects;
using Remora.Rest.Core;
using Remora.Results;

namespace TagR.Application.Services.Abstractions;

public interface IDiscordMessageService
{
    Task<Result<IMessage>> CreateMessageAsync(Snowflake channelId, string content, CancellationToken ct = default);
}