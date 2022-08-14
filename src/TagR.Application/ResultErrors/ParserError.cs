using Remora.Results;

namespace TagR.Application.ResultErrors;

public record ParserError(string parserErrorMessage) : ResultError(parserErrorMessage);
