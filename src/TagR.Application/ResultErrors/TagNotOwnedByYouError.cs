using Remora.Results;

namespace TagR.Application.ResultErrors;

public record TagNotOwnedByYouError() : ResultError("Couldn't delete the tag. You are not the tag owner.");