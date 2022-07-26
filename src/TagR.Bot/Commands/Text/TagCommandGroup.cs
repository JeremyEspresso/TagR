﻿using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.API.Objects;
using Remora.Discord.Commands.Contexts;
using Remora.Results;
using TagR.Application.Services.Abstractions;
using TagR.Bot.Commands.Conditions.Attributes;

namespace TagR.Bot.Commands.Text;

[Group("tag")]
public class TagCommandGroup : CommandGroup
{
    private readonly MessageContext _ctx;
    private readonly ITagService _tagService;
    private readonly ITagAliasService _tagAliasService;
    private readonly IDiscordMessageService _messageService;
    private readonly ILogger<TagCommandGroup> _logger;

    public TagCommandGroup
    (
        ICommandContext ctx,
        ITagService tagService,
        ITagAliasService tagAliasService,
        IDiscordMessageService messageService,
        ILogger<TagCommandGroup> logger
    )
    {
        _ctx = (MessageContext)ctx;
        _tagService = tagService;
        _tagAliasService = tagAliasService;
        _messageService = messageService;
        _logger = logger;
    }

    [Command("create")]
    public async Task<IResult> Create(string tagName, [Greedy] string content)
    {
        var tagCreate = await _tagService.CreateTagAsync(tagName, content, _ctx.User.ID, CancellationToken);
        await _messageService.CreateMessageAsync
            (
                _ctx.ChannelID,
                tagCreate.IsSuccess
                    ? $"Tag `{tagName}` successfully created."
                    : tagCreate.Error.Message,
                new MessageReference
                    (
                        _ctx.MessageID,
                        _ctx.ChannelID,
                        _ctx.GuildID,
                        false
                    ),
                true,
                CancellationToken
            );

        return Result.FromSuccess();
    }
    
    [Command("edit")]
    public async Task<IResult> Edit(string tagName, [Greedy] string newContent)
    {
        var tagCreate = await _tagService.UpdateTagAsync(tagName, newContent, _ctx.User.ID, CancellationToken);
        await _messageService.CreateMessageAsync
            (
                _ctx.ChannelID,
                tagCreate.IsSuccess
                    ? $"Tag `{tagName}` successfully edited."
                    : tagCreate.Error.Message,
                new MessageReference
                    (
                        _ctx.MessageID,
                        _ctx.ChannelID,
                        _ctx.GuildID,
                        false
                    ),
                true,
                CancellationToken
            );

        return Result.FromSuccess();
    }
    
    [Command("delete")]
    public async Task<IResult> Delete(string tagName)
    {
        var tagDelete = await _tagService.DeleteTagAsync(tagName, _ctx.User.ID, CancellationToken);

        await _messageService.CreateMessageAsync
            (
                _ctx.ChannelID,
                tagDelete.IsSuccess
                    ? $"Succesfully deleted `{tagName}`."
                    : tagDelete.Error.Message,
                new MessageReference
                    (
                        _ctx.MessageID,
                        _ctx.ChannelID,
                        _ctx.GuildID,
                        false
                    ),
                true,
                CancellationToken
            );

        return Result.FromSuccess();
    }

    [RequireModerator]
    [Command("enable")]
    public async Task<IResult> Enable(string tagName)
    {
        var tagDelete = await _tagService.EnableTagAsync(tagName, _ctx.User.ID, CancellationToken);

        await _messageService.CreateMessageAsync
            (
                _ctx.ChannelID,
                tagDelete.IsSuccess
                    ? $"Succesfully enabled `{tagName}`."
                    : tagDelete.Error.Message,
                new MessageReference
                    (
                        _ctx.MessageID,
                        _ctx.ChannelID,
                        _ctx.GuildID,
                        false
                    ),
                true,
                CancellationToken
            );

        return Result.FromSuccess();
    }

    [RequireModerator]
    [Command("disable")]
    public async Task<IResult> Disable(string tagName)
    {
        var tagDelete = await _tagService.DisableTagAsync(tagName, _ctx.User.ID, CancellationToken);

        await _messageService.CreateMessageAsync
            (
                _ctx.ChannelID,
                tagDelete.IsSuccess
                    ? $"Succesfully disabled `{tagName}`."
                    : tagDelete.Error.Message,
                new MessageReference
                    (
                        _ctx.MessageID,
                        _ctx.ChannelID,
                        _ctx.GuildID,
                        false
                    ),
                true,
                CancellationToken
            );

        return Result.FromSuccess();
    }

    [Command("alias")]
    public async Task<IResult> Alias(string targetTag, string tagName)
    {
        var x = await _tagAliasService.CreateAliasAsync(tagName, targetTag, _ctx.User.ID, CancellationToken);
        if (!x.IsSuccess)
        {
            // TODO
            return new Result();
        }

        return await _messageService.CreateMessageAsync(_ctx.ChannelID, "Created alias xdd",
            new MessageReference
            (
                _ctx.MessageID,
                _ctx.ChannelID,
                _ctx.GuildID,
                false
            ),
            false,
            CancellationToken
        );
    }
}

