using Remora.Rest.Core;
using Remora.Results;
using TagR.Domain.Moderation;

namespace TagR.Application.Services.Abstractions;

public interface IPermissionService
{
    Task<Result> IsModerator(Snowflake userId, CancellationToken ct = default);

    Task<Result> IsActionBlockedAsync(Snowflake userId, BlockedAction blockedActions, CancellationToken ct = default);
}
