using Remora.Rest.Core;
using Remora.Results;
using TagR.Domain.Moderation;

namespace TagR.Application.Services.Abstractions;

public interface IModService
{
    Task<IResult> BlockUserAsync(Snowflake userId, string? reason, CancellationToken ct = default);

    Task<Optional<BlockedUser>> GetBlockedUserBySnowflakeAsync(Snowflake userId, CancellationToken ct = default);

    Task<IResult> UnblockUserAsync(Snowflake userId, CancellationToken ct = default);
}

