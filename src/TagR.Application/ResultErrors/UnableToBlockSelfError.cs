using Remora.Results;

namespace TagR.Application.ResultErrors;

public record UnableToBlockSelfError() : ResultError($"You can't block yourself dummy.");
