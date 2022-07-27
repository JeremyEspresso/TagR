using Microsoft.EntityFrameworkCore;
using Remora.Rest.Core;
using Remora.Results;
using TagR.Database;
using TagR.Domain;
using TagR.Application.ResultErrors;
using TagR.Application.Services.Abstractions;

namespace TagR.Application.Services;

public class TagService : ITagService
{
    private readonly TagRDbContext _context;

    public TagService(TagRDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Tag>> CreateTagAsync(string tagName, string content, Snowflake actorId, CancellationToken ct = default)
    {
        var newTag = new Tag
        {
            Name = tagName,
            Content = content,
            OwnerDiscordSnowflake = actorId,
        };

        var getTag = await GetTagByName(tagName, ct);
        if(getTag.IsDefined(out var _))
        {
            return Result<Tag>.FromError(new TagExistsError());
        }

        _context.Tags.Add(newTag);
        await _context.SaveChangesAsync(ct);

        return newTag;
    }

    public async Task<Result<Tag>> UpdateTagAsync(string tagName, string newContent, Snowflake actorId, bool isMod, CancellationToken ct = default)
    {
        var getTag = await GetTagByName(tagName, ct);
        if (!getTag.IsDefined(out var tag))
        {
            return Result<Tag>.FromError(new TagNotFoundError());
        }

        if (!isMod && tag.OwnerDiscordSnowflake != actorId)
        {
            return Result<Tag>.FromError(new TagNotOwnedByYouError());
        }

        tag.Content = newContent;
        
        _context.Tags.Update(tag);
        await _context.SaveChangesAsync(ct);

        return tag;
    }

    public async Task<Result> DeleteTagAsync(string tagName, Snowflake actorId, bool isMod, CancellationToken ct = default)
    {
        var getTag = await GetTagByName(tagName, ct);
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

        return Result.FromSuccess();
    }

    public async Task<Result> EnableTagAsync(string tagName, Snowflake actorId, bool isMod, CancellationToken ct = default)
    {
        var getTag = await GetTagByName(tagName, ct);
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

        return Result.FromSuccess();
    }

    public async Task<Result> DisableTagAsync(string tagName, Snowflake actorId, bool isMod, CancellationToken ct = default)
    {
        var getTag = await GetTagByName(tagName, ct);
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

        return Result.FromSuccess();
    }

    public async Task<Result> IncrementTagUseAsync(Tag tag, CancellationToken ct = default)
    {
        tag.Uses++;
        
        _context.Tags.Update(tag);
        await _context.SaveChangesAsync(ct);
        
        return Result.FromSuccess();
    }

    public async Task<Optional<Tag>> GetTagByName(string tagName, CancellationToken ct = default)
    {
        return (await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagName, ct))!;
    }
}
