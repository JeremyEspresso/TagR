using Remora.Results;

namespace TagR.Application.ResultErrors;

public record TagNotFoundError() : ResultError("Tag not found");