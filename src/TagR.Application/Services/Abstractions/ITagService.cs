using Remora.Rest.Core;
using Remora.Results;
using TagR.Domain;

namespace TagR.Application.Services.Abstractions;

public interface ITagService
{
    Task<Result<Tag>> CreateTagAsync(string tagName, string content, Snowflake actorId, CancellationToken ct = default);

    Task<Result<Tag>> UpdateTagAsync(string tagName, string newContent, Snowflake actorId, CancellationToken ct = default);

    Task<Result> DeleteTagAsync(string tagName, Snowflake actorId, CancellationToken ct = default);

    Task<Result> ToggleTagAsync(string tagName, Snowflake actorId, CancellationToken ct = default);

    Task<Optional<Tag>> GetTagByName(string tagName, CancellationToken ct = default);
}
