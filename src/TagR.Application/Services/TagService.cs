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

    public async Task<Result<Tag>> UpdateTagAsync(string tagName, string newContent, Snowflake actorId, CancellationToken ct = default)
    {
        var getTag = await GetTagByName(tagName, ct);
        if (!getTag.IsDefined(out var tag))
        {
            return Result<Tag>.FromError(new TagNotFoundError());
        }

        tag.Content = newContent;
        
        _context.Tags.Update(tag);
        await _context.SaveChangesAsync(ct);

        return tag;
    }

    // TODO: Mods should be able to delete others their tags
    public async Task<Result> DeleteTagAsync(string tagName, Snowflake actorId, CancellationToken ct = default)
    {
        var getTag = await GetTagByName(tagName, ct);
        if (!getTag.IsDefined(out var tag))
        {
            return Result.FromError(new TagNotFoundError());
        }

        if (tag.OwnerDiscordSnowflake != actorId)
        {
            return Result.FromError(new TagNotOwnedByYouError());
        }

        _context.Tags.Remove(tag);
        await _context.SaveChangesAsync(ct);

        return Result.FromSuccess();
    }

    // TODO: Mod only
    public async Task<Result> ToggleTagAsync(string tagName, Snowflake actorId, CancellationToken ct = default)
    {
        var getTag = await GetTagByName(tagName, ct);
        if (!getTag.IsDefined(out var tag))
        {
            return Result.FromError(new TagNotFoundError());
        }

        tag.Disabled = !tag.Disabled;
        _context.Tags.Update(tag);
        await _context.SaveChangesAsync(ct);

        return Result.FromSuccess();
    }

    public async Task<Optional<Tag>> GetTagByName(string tagName, CancellationToken ct = default)
    {
        return (await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagName, ct))!;
    }
}
