using Remora.Rest.Core;
using Remora.Results;
using TagR.Domain;

namespace TagR.Application.Services.Abstractions;

public interface ITagService
{
    Task<Result<Tag>> CreateTagAsync(string tagName, string content, Snowflake actorId, CancellationToken ct = default);

    Task<Result<Tag>> UpdateTagAsync(string tagName, string newContent, Snowflake actorId, bool isMod, CancellationToken ct = default);

    Task<Result> DeleteTagAsync(string tagName, Snowflake actorId, bool isMod, CancellationToken ct = default);

    Task<Result> EnableTagAsync(string tagName, Snowflake actorId, bool isMod, CancellationToken ct = default);

    Task<Result> DisableTagAsync(string tagName, Snowflake actorId, bool isMod, CancellationToken ct = default);

    Task<Result> IncrementTagUseAsync(Tag tag, CancellationToken ct = default);

    Task<Optional<Tag>> GetTagByNameAsync(string tagName, CancellationToken ct = default);
}
