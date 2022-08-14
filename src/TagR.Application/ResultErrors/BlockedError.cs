using Remora.Results;

namespace TagR.Application.ResultErrors;

public record BlockedError(string message) : ResultError(message);