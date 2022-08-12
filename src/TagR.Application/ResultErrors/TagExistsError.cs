using Remora.Results;

namespace TagR.Application.ResultErrors;

public record TagWithNameExistsError() : ResultError("Tag with this name already exists");

public record TagWithContentExistsError(string tagName) : ResultError($"Tag with this exact content already exists. See tag: `{tagName}`");

public record TagEditedWithContentExistsError(string tagName) : ResultError($"Tag with this exact content already exists. See tag: `{tagName}`.\nYour edit was reverted.");