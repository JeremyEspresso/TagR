using Remora.Discord.API.Abstractions.Objects;
using Remora.Rest.Core;

namespace TagR.Application.Services.Abstractions;

public interface IMessageProcessingService
{
    Task ProcessMessageAsync(Snowflake channelId, Snowflake messageId, string messageContent, Optional<IMessageReference> referencedMessage, CancellationToken ct = default);
}

