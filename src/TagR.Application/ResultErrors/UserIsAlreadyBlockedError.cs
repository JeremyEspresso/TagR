using Remora.Results;
using TagR.Domain.Moderation;

namespace TagR.Application.ResultErrors;

public record UserIsAlreadyBlockedError(BlockedUser BlockedUser) : ResultError(string.Empty);