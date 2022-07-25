using Remora.Rest.Core;

namespace TagR.Application.Services;

public interface IMessageProcessingService
{
    Task ProcessMessageAsync(Snowflake channelId, string messageContent, CancellationToken ct = default);
}

