﻿using Microsoft.EntityFrameworkCore;
using OneOf;
using Remora.Rest.Core;
using Remora.Results;
using TagR.Application.Common.Hashing;
using TagR.Database;
using TagR.Domain;
using TagR.Application.ResultErrors;
using TagR.Application.Services.Abstractions;
using TagR.Application.Entities.Auditing;
using TagR.Domain.Moderation;

namespace TagR.Application.Services;

public class TagService : ITagService
{
    private readonly TagRDbContext _context;
    private readonly IAuditLogger _auditLogger;
    private readonly IClock _clock;
    private readonly IPermissionService _permissionService;

    public TagService(TagRDbContext context, IAuditLogger auditLogger, IClock clock, IPermissionService permissionService)
    {
        _context = context;
        _auditLogger = auditLogger;
        _clock = clock;
        _permissionService = permissionService;
    }

    public async Task<Result<Tag>> CreateTagAsync(string tagName, string content, Snowflake actorId, CancellationToken ct = default)
    {
        var blocked = await _permissionService.IsActionBlockedAsync(actorId, BlockedAction.TagCreate, ct);
        if (blocked)
        {
            return Result<Tag>.FromError(new BlockedError("You are blocked from creating tags."));
        }
        
        var getTagByName = await GetTagByNameAsync(tagName, ct);
        if(getTagByName.IsDefined(out var _))
        {
            return Result<Tag>.FromError(new TagWithNameExistsError());
        }

        var getTagByContent = await GetTagByContentAsync(content, ct);
        if (getTagByContent.IsDefined(out var t))
        {
            return Result<Tag>.FromError(new TagWithContentExistsError(t.Name));
        }

        var timestamp = _clock.UtcNow;
        var revision = CreateRevision(content, timestamp);
        
        var newTag = new Tag
        {
            Name = tagName,
            Revisions = new TagRevision[]
            {
                revision
            },
            OwnerDiscordSnowflake = actorId,
            CreatedAtUtc = timestamp,
        };

        _context.Tags.Add(newTag);
        await _context.SaveChangesAsync(ct);

        await _auditLogger.Log
            (
                new TagCreatedEvent
                (
                  newTag.Id,
                  actorId
                )
            );

        return newTag;
    }

    public async Task<Result<Tag>> UpdateTagAsync(string tagName, string newContent, Snowflake actorId, CancellationToken ct = default)
    {
        var blocked = await _permissionService.IsActionBlockedAsync(actorId, BlockedAction.TagEdit, ct);
        if (blocked)
        {
            return Result<Tag>.FromError(new BlockedError("You are blocked from editing tags."));
        }
        
        var getTag = await GetTagByNameAsync(tagName, ct);
        if (!getTag.IsDefined(out var tag))
        {
            return Result<Tag>.FromError(new TagNotFoundError());
        }

        var isMod = await _permissionService.IsModerator(actorId, ct);
        
        if (!isMod && tag.OwnerDiscordSnowflake != actorId)
        {
            return Result<Tag>.FromError(new TagNotOwnedByYouError());
        }

        var getTagByContent = await GetTagByContentAsync(newContent, ct);
        if (getTagByContent.IsDefined(out var t))
        {
            return Result<Tag>.FromError(new TagEditedWithContentExistsError(t.Name));
        }

        var previousRevision = tag.Revisions.Last();

        var timestamp = _clock.UtcNow;
        var revision = CreateRevision(newContent, timestamp);
        
        tag.Revisions.Add(revision);
        
        _context.Tags.Update(tag);
        await _context.SaveChangesAsync(ct);

        await _auditLogger.Log
        (
            new TagUpdatedEvent
            (
              tag.Id,
              actorId,
              previousRevision.Hash,
              revision.Hash
            ),
            ct
        );

        return tag;
    }

    public async Task<Result> DeleteTagAsync(string tagName, Snowflake actorId, CancellationToken ct = default)
    {
        var blocked = await _permissionService.IsActionBlockedAsync(actorId, BlockedAction.TagDelete, ct);
        if (blocked)
        {
            return Result.FromError(new BlockedError("You are blocked from deleting tags."));
        }
        
        var getTag = await GetTagByNameAsync(tagName, ct);
        if (!getTag.IsDefined(out var tag))
        {
            return Result.FromError(new TagNotFoundError());
        }
        
        var isMod = await _permissionService.IsModerator(actorId, ct);
        
        if (!isMod && tag.OwnerDiscordSnowflake != actorId)
        {
            return Result.FromError(new TagNotOwnedByYouError());
        }

        _context.Tags.Remove(tag);
        await _context.SaveChangesAsync(ct);

        await _auditLogger.Log
        (
            new TagDeletedEvent
            (
              tag.Id,
              actorId,
              tag.Name,
              tag.Content
            ),
            ct
        );

        return Result.FromSuccess();
    }

    public async Task<Result> EnableTagAsync(string tagName, Snowflake actorId, CancellationToken ct = default)
    {
        var getTag = await GetTagByNameAsync(tagName, ct);
        if (!getTag.IsDefined(out var tag))
        {
            return Result.FromError(new TagNotFoundError());
        }
        
        var isMod = await _permissionService.IsModerator(actorId, ct);

        if (!isMod)
        {
            return Result.FromError(new InsufficientPermissionsError());
        }

        tag.Disabled = false;
        _context.Tags.Update(tag);
        await _context.SaveChangesAsync(ct);

        await _auditLogger.Log
        (
            new TagEnabledEvent
            (
              tag.Id,
              actorId
            ),
            ct
        );

        return Result.FromSuccess();
    }

    public async Task<Result> DisableTagAsync(string tagName, Snowflake actorId, CancellationToken ct = default)
    {
        var getTag = await GetTagByNameAsync(tagName, ct);
        if (!getTag.IsDefined(out var tag))
        {
            return Result.FromError(new TagNotFoundError());
        }
        
        var isMod = await _permissionService.IsModerator(actorId, ct);

        if (!isMod)
        {
            return Result.FromError(new InsufficientPermissionsError());
        }

        tag.Disabled = true;
        _context.Tags.Update(tag);
        await _context.SaveChangesAsync(ct);

        await _auditLogger.Log
        (
            new TagDisabledEvent
            (
              tag.Id,
              actorId
            ),
            ct
        );

        return Result.FromSuccess();
    }

    public async Task<Result> AddTagUseAsync(Tag tag, Snowflake channelId, Snowflake userId, DateTime usedAtUtc, CancellationToken ct = default)
    {
        var tagUse = new TagUse
        {
            UserSnowflake = userId,
            ChannelSnowflake = channelId,
            DateTimeUtc = usedAtUtc
        };
        
        tag.Uses.Add(tagUse);
        
        _context.Tags.Update(tag);
        await _context.SaveChangesAsync(ct);
        
        return Result.FromSuccess();
    }

    public async Task<Optional<Tag>> GetTagByNameAsync(string tagName, CancellationToken ct = default)
    {
        return (await _context.Tags
            .Include(t => t.AuditLogs)
            .Include(t => t.Revisions)
            .Include(t => t.Uses)
            .FirstOrDefaultAsync(t => t.Name == tagName, ct))!;
    }

    public Task<bool> TagExitsByName(string name, CancellationToken ct = default)
    {
        return _context.Tags
            .Include(t => t.Aliases)
            .AnyAsync(t => t.Name == name || t.Aliases.Any(ta => ta.Name == name), ct);
    }

    private async Task<Optional<Tag>> GetTagByContentAsync(string content, CancellationToken ct = default)
    {
        var x = await _context.Revisions.Where(r => r.Content == content)
                .Include(t => t.Tag)
                .FirstOrDefaultAsync(ct);
        
        return x?.Tag!;
    }

    private TagRevision CreateRevision(string content, DateTime timestamp)
    {
        var contentHash = HashHelper.HashSha1($"{content}-{timestamp}");
        var shortContentHash = contentHash[..7]; // TODO:  Add this as a constant somewhere
        
        return new TagRevision
        {
            Content = content,
            Hash = contentHash,
            ShortHash = shortContentHash,
            TimestampUtc = timestamp
        };
    }
}
