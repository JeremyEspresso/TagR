using Remora.Results;

namespace TagR.Application.ResultErrors;

public record ParserError(string token) : ResultError($"Unknown token `{token}`");
