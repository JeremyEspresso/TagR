using Remora.Rest.Core;
using Remora.Results;

namespace TagR.Application.ResultErrors;

public record UserIsNotBlockedError(Snowflake userSnowflake) : ResultError($"User `{userSnowflake}` is not blocked.");