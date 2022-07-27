using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.Commands.Contexts;
using Remora.Results;
using TagR.Application.Services.Abstractions;

namespace TagR.Bot.Commands.Text;

[Group("tag")]
public class TagCommandGroup : CommandGroup
{
    private readonly ICommandContext _ctx;
    private readonly ITagService _tagService;
    private readonly IDiscordMessageService _messageService;
    private readonly ILogger<TagCommandGroup> _logger;

    public TagCommandGroup
    (
        ICommandContext ctx, 
        ITagService tagService, 
        IDiscordMessageService messageService, 
        ILogger<TagCommandGroup> logger
    )
    {
        _ctx = ctx;
        _tagService = tagService;
        _messageService = messageService;
        _logger = logger;
    }

    // TODO Anyone who isn't blocked.
    [Command("create")]
    public async Task<IResult> Create(string tagName, [Greedy]string content)
    {
        var tagCreate = await _tagService.CreateTagAsync(tagName, content, _ctx.User.ID, CancellationToken);
        await _messageService.CreateMessageAsync
            (
                _ctx.ChannelID, 
                tagCreate.IsSuccess 
                    ? $"Tag `{tagName}` successfully created." 
                    : tagCreate.Error.Message, 
                CancellationToken
            );

        return Result.FromSuccess();
    }

    // TODO TagOwner or Mod
    [Command("edit")]
    public async Task<IResult> Edit(string tagName, [Greedy]string newContent)
    {
        var tagCreate = await _tagService.UpdateTagAsync(tagName, newContent, _ctx.User.ID, /*TODO*/true, CancellationToken);
        await _messageService.CreateMessageAsync
            (
                _ctx.ChannelID,
                tagCreate.IsSuccess
                    ? $"Tag `{tagName}` successfully edited."
                    : tagCreate.Error.Message,
                CancellationToken
            );

        return Result.FromSuccess();
    }

    // TODO TagOwner or Mod
    [Command("delete")]
    public async Task<IResult> Delete(string tagName)
    {
        var tagDelete = await _tagService.DeleteTagAsync(tagName, _ctx.User.ID, /*TODO*/true, CancellationToken);

        await _messageService.CreateMessageAsync
            (
                _ctx.ChannelID,
                tagDelete.IsSuccess 
                    ? $"Succesfully deleted `{tagName}`." 
                    : tagDelete.Error.Message, 
                CancellationToken
            );

        return Result.FromSuccess();
    }

    // TODO Mod only
    [Command("enable")]
    public async Task<IResult> Enable(string tagName)
    {
        var tagDelete = await _tagService.EnableTagAsync(tagName, _ctx.User.ID, /*TODO*/true, CancellationToken);

        await _messageService.CreateMessageAsync
            (
                _ctx.ChannelID,
                tagDelete.IsSuccess
                    ? $"Succesfully enabled `{tagName}`."
                    : tagDelete.Error.Message,
                CancellationToken
            );

        return Result.FromSuccess();
    }

    // TODO Mod only
    [Command("disable")]
    public async Task<IResult> Disable(string tagName)
    {
        var tagDelete = await _tagService.DisableTagAsync(tagName, _ctx.User.ID, /*TODO*/true, CancellationToken);

        await _messageService.CreateMessageAsync
            (
                _ctx.ChannelID,
                tagDelete.IsSuccess
                    ? $"Succesfully disabled `{tagName}`."
                    : tagDelete.Error.Message,
                CancellationToken
            );

        return Result.FromSuccess();
    }

    [Command("alias")]
    public async Task<IResult> Alias(string tagName, string nameOfAliasedTag)
    {
        throw new NotImplementedException();
    }
}

