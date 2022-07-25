using Remora.Results;

namespace TagR.Application.ResultErrors;

public record TagExistsError() : ResultError("Tag with this name already exists");
