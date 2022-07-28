﻿using Remora.Commands.Services;
using Remora.Commands.Trees.Nodes;
using Remora.Rest.Core;
using TagR.Application.Services.Abstractions;

namespace TagR.Application.Services;

public class MessageProcessingService : IMessageProcessingService
{
    private readonly ITagService _tagService;
    private readonly IDiscordMessageService _messageService;
    private readonly ILogger<MessageProcessingService> _logger;

    private readonly IReadOnlyList<IChildNode> _commandGroupNames;

    public MessageProcessingService
    (
        ITagService tagService,
        IDiscordMessageService messageService,
        CommandService commandService,
        ILogger<MessageProcessingService> logger
    )
    {
        _tagService = tagService;
        _messageService = messageService;
        _logger = logger;

        _commandGroupNames = GetCommandGroups(commandService);
    }

    public async Task ProcessMessageAsync(Snowflake channelId, Snowflake messageId, string messageContent, CancellationToken ct = default)
    {
        _logger.LogInformation("Processing message id: {messageId} ", messageId);

        var content = StripPrefix(messageContent);

        if (_commandGroupNames.Any(c => content.StartsWith(c.Key)))
            return;

        var getTag = await _tagService.GetTagByNameAsync(content);

        if (!getTag.IsDefined(out var tag))
        {
            await _messageService.CreateMessageAsync(channelId, TagNotFoundText, ct);
            return;
        }

        if (tag!.Disabled)
            return;

        await _messageService.CreateMessageAsync(channelId, tag!.Content, ct);
        await _tagService.IncrementTagUseAsync(tag, ct);
    }

    private string StripPrefix(string messageContent)
    {
        return messageContent[1..];
    }

    private IReadOnlyList<IChildNode> GetCommandGroups(CommandService cmdService)
    {
        return cmdService.TreeAccessor.TryGetNamedTree(null, out var tree)
            ? tree.Root.Children
            : throw new InvalidOperationException("Couldn't retrieve command tree.");
    }

    private const string TagNotFoundText = "Tag not found";
}

