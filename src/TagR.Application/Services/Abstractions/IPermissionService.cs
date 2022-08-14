using Remora.Rest.Core;
using Remora.Results;
using TagR.Domain.Moderation;

namespace TagR.Application.Services.Abstractions;

public interface IPermissionService
{
    Task<bool> IsModerator(Snowflake userId, CancellationToken ct = default);

    Task<bool> IsActionBlockedAsync(Snowflake userId, BlockedAction blockedActions, CancellationToken ct = default);
}
