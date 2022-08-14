using Remora.Rest.Core;
using Remora.Results;
using TagR.Domain;

namespace TagR.Application.Services.Abstractions;

public interface ITagAliasService
{
    Task<Result<TagAlias>> CreateAliasAsync(string aliasName, string tagTargetName, Snowflake actorId, CancellationToken ct = default);
    
    Task<Result<TagAlias>> UpdateAliasNameAsync(string aliasName, string newName, Snowflake actorId, CancellationToken ct = default);
    
    Task<Result<TagAlias>> UpdateAliasTargetAsync(string aliasName, string newTarget, Snowflake actorId, CancellationToken ct = default);
    
    Task<Result> DeleteAliasAsync(string aliasName, Snowflake actorId, CancellationToken ct = default);
    
    Task<Optional<TagAlias>> GetTagAliasByNameAsync(string aliasName, CancellationToken ct = default);
    
    Task AddTagAliasUseAsync(TagAlias alias, Snowflake channelId, Snowflake actorId, DateTime clockUtcNow, CancellationToken ct);
}