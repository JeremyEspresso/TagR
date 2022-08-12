using Remora.Rest.Core;
using Remora.Results;
using TagR.Domain.Moderation;

namespace TagR.Application.Services.Abstractions;

public interface IModService
{
    Task<Result> BlockUserAsync(Snowflake userId, BlockedAction blockedActions, Snowflake actor, CancellationToken ct = default);

    Task<Optional<BlockedUser>> GetBlockedUserBySnowflakeAsync(Snowflake userId, CancellationToken ct = default);

    Task<Result> UnblockUserAsync(Snowflake userId, BlockedAction restoredActions, Snowflake actor, CancellationToken ct = default);
}

