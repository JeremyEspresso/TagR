using Remora.Results;
using TagR.Domain.Moderation;

namespace TagR.Application.ResultErrors;

public record UserIsAlreadyBlockedForActionsError(BlockedUser BlockedUser) : ResultError(string.Empty);