using Remora.Results;

namespace TagR.Application.ResultErrors;

public record InsufficientPermissionsError() : ResultError("Insuffucient permissions.");