using Remora.Rest.Core;

namespace TagR.Application.Services.Abstractions;

public interface IMessageProcessingService
{
    Task ProcessMessageAsync(Snowflake channelId, string messageContent, CancellationToken ct = default);
}

