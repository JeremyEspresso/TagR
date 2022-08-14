using Microsoft.EntityFrameworkCore;
using Remora.Rest.Core;
using Remora.Results;
using TagR.Application.ResultErrors;
using TagR.Application.Services.Abstractions;
using TagR.Database;
using TagR.Domain;
using TagR.Domain.Moderation;

namespace TagR.Application.Services;

public class TagAliasService : ITagAliasService
{
    private readonly TagRDbContext _context;
    private readonly ITagService _tagService;
    private readonly IPermissionService _permissionService;
    
    public TagAliasService(TagRDbContext context, ITagService tagService, IPermissionService permissionService)
    {
        _context = context;
        _tagService = tagService;
        _permissionService = permissionService;
    }

    public async Task<Result<TagAlias>> CreateAliasAsync(string aliasName, string tagTargetName, Snowflake actorId, CancellationToken ct = default)
    {
        var blocked = await _permissionService.IsActionBlockedAsync(actorId, BlockedAction.AliasCreate, ct);
        if (blocked)
        {
            return Result<TagAlias>.FromError(new BlockedError("You are blocked from creating aliases."));
        }

        var existsByName = await _tagService.TagExitsByName(aliasName, ct);
        if (existsByName)
        {
            return Result<TagAlias>.FromError(new TagWithNameExistsError());
        }

        var getTagByName = await _tagService.GetTagByNameAsync(tagTargetName, ct);
        if (!getTagByName.IsDefined(out var tagTarget))
        {
            return Result<TagAlias>.FromError(new TagNotFoundError());
        }

        var alias = new TagAlias
        {
            Name = aliasName,
            Parent = tagTarget!
        };

        _context.Aliases.Add(alias);
        await _context.SaveChangesAsync(ct);

        return alias;
    }

    public async Task<Result<TagAlias>> UpdateAliasNameAsync(string aliasName, string newName, Snowflake actorId, CancellationToken ct = default)
    {
        var blocked = await _permissionService.IsActionBlockedAsync(actorId, BlockedAction.AliasEdit, ct);
        if (blocked)
        {
            return Result<TagAlias>.FromError(new BlockedError("You are blocked from editing aliases."));
        }

        var getAliasByName = await GetTagAliasByNameAsync(aliasName, ct);
        if (!getAliasByName.IsDefined(out var tagAlias))
        {
            return Result<TagAlias>.FromError(new TagNotFoundError());
        }
        
        var existsByName = await _tagService.TagExitsByName(newName, ct);
        if (existsByName)
        {
            return Result<TagAlias>.FromError(new TagWithNameExistsError());
        }

        tagAlias.Name = newName;

        _context.Aliases.Update(tagAlias);
        await _context.SaveChangesAsync(ct);

        return tagAlias;
    }
    
    public async Task<Result<TagAlias>> UpdateAliasTargetAsync(string aliasName, string newTarget, Snowflake actorId, CancellationToken ct = default)
    {
        var blocked = await _permissionService.IsActionBlockedAsync(actorId, BlockedAction.AliasEdit, ct);
        if (blocked)
        {
            return Result<TagAlias>.FromError(new BlockedError("You are blocked from editing aliases."));
        }

        var getAliasByName = await GetTagAliasByNameAsync(aliasName, ct);
        if (!getAliasByName.IsDefined(out var tagAlias))
        {
            return Result<TagAlias>.FromError(new TagNotFoundError());
        }
        
        var getTagByName = await _tagService.GetTagByNameAsync(newTarget, ct);
        if (!getTagByName.IsDefined(out var targetTag))
        {
            return Result<TagAlias>.FromError(new TagNotFoundError());
        }

        tagAlias.Parent = targetTag;

        _context.Aliases.Update(tagAlias);
        await _context.SaveChangesAsync(ct);

        return tagAlias;
    }

    public async Task<Result> DeleteAliasAsync(string aliasName, Snowflake actorId, CancellationToken ct = default)
    {
        var blocked = await _permissionService.IsActionBlockedAsync(actorId, BlockedAction.AliasDelete, ct);
        if (blocked)
        {
            return Result.FromError(new BlockedError("You are blocked from deleting aliases."));
        }
        
        var getAliasByName = await GetTagAliasByNameAsync(aliasName, ct);
        if (!getAliasByName.IsDefined(out var tagAlias))
        {
            return Result.FromError(new TagNotFoundError());
        }

        _context.Aliases.Remove(tagAlias);
        await _context.SaveChangesAsync(ct);
        
        return Result.FromSuccess();
    }

    public async Task<Optional<TagAlias>> GetTagAliasByNameAsync(string aliasName, CancellationToken ct = default)
    {
        return (await _context.Aliases
            .Include(ta => ta.Parent)
            .Include(ta => ta.Parent.Revisions)
            .Include(ta => ta.Uses)
            .FirstOrDefaultAsync(ta => ta.Name == aliasName, ct))!;
    }

    public async Task AddTagAliasUseAsync(TagAlias alias, Snowflake channelId, Snowflake actorId, DateTime clockUtcNow, CancellationToken ct)
    {
        var tagAliasUse = new TagAliasUse
        {
            UserSnowflake = actorId,
            ChannelSnowflake = channelId,
            DateTimeUtc = clockUtcNow
        };
        
        alias.Uses.Add(tagAliasUse);

        _context.Aliases.Update(alias);
        await _context.SaveChangesAsync(ct);
    }
}