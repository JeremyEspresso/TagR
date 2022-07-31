using Microsoft.EntityFrameworkCore;
using Remora.Rest.Core;
using Remora.Results;
using TagR.Database;
using TagR.Domain;
using TagR.Application.ResultErrors;
using TagR.Application.Services.Abstractions;
using TagR.Application.Entities.Auditing;

namespace TagR.Application.Services;

public class TagService : ITagService
{
    private readonly TagRDbContext _context;
    private readonly IAuditLogger _auditLogger;
    private readonly IClock _clock;

    public TagService(TagRDbContext context, IAuditLogger auditLogger, IClock clock)
    {
        _context = context;
        _auditLogger = auditLogger;
        _clock = clock;
    }

    public async Task<Result<Tag>> CreateTagAsync(string tagName, string content, Snowflake actorId, CancellationToken ct = default)
    {
        var newTag = new Tag
        {
            Name = tagName,
            Content = content,
            OwnerDiscordSnowflake = actorId,
            CreatedAtUtc = _clock.UtcNow,
        };

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

    public async Task<Result<Tag>> UpdateTagAsync(string tagName, string newContent, Snowflake actorId, bool isMod, CancellationToken ct = default)
    {
        var getTag = await GetTagByNameAsync(tagName, ct);
        if (!getTag.IsDefined(out var tag))
        {
            return Result<Tag>.FromError(new TagNotFoundError());
        }

        if (!isMod && tag.OwnerDiscordSnowflake != actorId)
        {
            return Result<Tag>.FromError(new TagNotOwnedByYouError());
        }

        var getTagByContent = await GetTagByContentAsync(newContent, ct);
        if (getTagByContent.IsDefined(out var t))
        {
            return Result<Tag>.FromError(new TagEditedWithContentExistsError(t.Name));
        }

        var oldContent = tag.Content;

        tag.Content = newContent;
        
        _context.Tags.Update(tag);
        await _context.SaveChangesAsync(ct);

        await _auditLogger.Log
        (
            new TagUpdatedEvent
            (
              tag.Id,
              actorId,
              oldContent,
              newContent
            )
        );

        return tag;
    }

    public async Task<Result> DeleteTagAsync(string tagName, Snowflake actorId, bool isMod, CancellationToken ct = default)
    {
        var getTag = await GetTagByNameAsync(tagName, ct);
        if (!getTag.IsDefined(out var tag))
        {
            return Result.FromError(new TagNotFoundError());
        }

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
            )
        );

        return Result.FromSuccess();
    }

    public async Task<Result> EnableTagAsync(string tagName, Snowflake actorId, bool isMod, CancellationToken ct = default)
    {
        var getTag = await GetTagByNameAsync(tagName, ct);
        if (!getTag.IsDefined(out var tag))
        {
            return Result.FromError(new TagNotFoundError());
        }

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
            )
        );

        return Result.FromSuccess();
    }

    public async Task<Result> DisableTagAsync(string tagName, Snowflake actorId, bool isMod, CancellationToken ct = default)
    {
        var getTag = await GetTagByNameAsync(tagName, ct);
        if (!getTag.IsDefined(out var tag))
        {
            return Result.FromError(new TagNotFoundError());
        }

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
            )
        );

        return Result.FromSuccess();
    }

    public async Task<Result> IncrementTagUseAsync(Tag tag, CancellationToken ct = default)
    {
        tag.Uses++;
        
        _context.Tags.Update(tag);
        await _context.SaveChangesAsync(ct);
        
        return Result.FromSuccess();
    }

    public async Task<Optional<Tag>> GetTagByNameAsync(string tagName, CancellationToken ct = default)
    {
        return (await _context.Tags.Include(t => t.AuditLogs).FirstOrDefaultAsync(t => t.Name == tagName, ct))!;
    }

    private async Task<Optional<Tag>> GetTagByContentAsync(string content, CancellationToken ct = default)
    {
        return (await _context.Tags.Include(t => t.AuditLogs).FirstOrDefaultAsync(t => t.Content == content, ct))!;
    }
}
