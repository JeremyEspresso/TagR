using Remora.Rest.Core;
using Remora.Results;

namespace TagR.Application.Services.Abstractions;

public interface IPermissionService
{
    Task<Result> IsModerator(Snowflake userId, CancellationToken ct = default);
}