using Remora.Results;

namespace TagR.Application.ResultErrors;

public record MessageError(string message) : ResultError(message);

