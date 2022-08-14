using Remora.Commands.Services;
using Remora.Commands.Trees.Nodes;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Objects;
using Remora.Rest.Core;
using TagR.Application.Services.Abstractions;
using TagR.Domain;
using TagR.Domain.Moderation;
using OneOf;
using Remora.Results;
using TagR.Application.ResultErrors;

namespace TagR.Application.Services;

public class MessageProcessingService : IMessageProcessingService
{
    private readonly ITagService _tagService;
    private readonly ITagAliasService _tagAliasService;
    private readonly IDiscordMessageService _messageService;
    private readonly IPermissionService _permissionService;
    private readonly IClock _clock;
    private readonly ILogger<MessageProcessingService> _logger;

    private readonly IReadOnlyList<IChildNode> _commandGroupNames;

    public MessageProcessingService
    (
        ITagService tagService,
        ITagAliasService tagAliasService,
        IDiscordMessageService messageService,
        IPermissionService permissionService,
        IClock clock,
        CommandService commandService,
        ILogger<MessageProcessingService> logger
    )
    {
        _tagService = tagService;
        _tagAliasService = tagAliasService;
        _messageService = messageService;
        _permissionService = permissionService;
        _clock = clock;
        _logger = logger;

        _commandGroupNames = GetCommandGroups(commandService);
    }

    public async Task ProcessMessageAsync(Snowflake channelId, Snowflake messageId, Snowflake actorId, string messageContent, Optional<IMessageReference> referencedMessage, CancellationToken ct = default)
    {
        _logger.LogInformation("Processing message id: {messageId} ", messageId);

        var content = StripPrefix(messageContent);

        if (_commandGroupNames.Any(c => content.StartsWith(c.Key)))
            return;

        var getMaybeTag = await GetTagOrTagAliasByNameAsync(content, ct);
        if (!getMaybeTag.IsSuccess)
        {
            await _messageService.CreateMessageAsync(channelId, TagNotFoundText, new MessageReference(messageId, channelId), true, ct);
            return;
        }
        
        var maybeTag = getMaybeTag.Entity!;
        var tag = maybeTag.TryPickT0(out var tagToUse, out var alias) ? tagToUse : alias.Parent;
        
        if (tag!.Disabled)
            return;

        var blocked = await _permissionService.IsActionBlockedAsync(actorId, BlockedAction.TagInvoke, ct);

        if (blocked)
        {
            // Don't waste an API call informing the user they are blocked. Vector for abuse.
           return;
        }

        var msgRef = new MessageReference(messageId, channelId, default, false);

        if (referencedMessage.IsDefined(out var reference))
        {
            msgRef = new MessageReference(reference.MessageID, reference.ChannelID, reference.GuildID, false);
        }

        await _messageService.CreateMessageAsync(channelId, tag!.Content, msgRef, false, ct);
        await _tagService.AddTagUseAsync(tag, channelId, actorId, _clock.UtcNow, ct);

        if (maybeTag.IsT1)
        {
            await _tagAliasService.AddTagAliasUseAsync(alias, channelId, actorId, _clock.UtcNow, ct);
        }
    }

    private static string StripPrefix(string messageContent)
    {
        return messageContent[1..];
    }

    private static IReadOnlyList<IChildNode> GetCommandGroups(CommandService cmdService)
    {
        return cmdService.TreeAccessor.TryGetNamedTree(null, out var tree)
            ? tree.Root.Children
            : throw new InvalidOperationException("Couldn't retrieve command tree.");
    }

    private async Task<Result<OneOf<Tag, TagAlias>>> GetTagOrTagAliasByNameAsync(string name, CancellationToken ct = default)
    {
        var getTag = await _tagService.GetTagByNameAsync(name, ct);
        if (getTag.IsDefined(out var tag))
        {
            return Result<OneOf<Tag, TagAlias>>.FromSuccess(tag);
        }

        var getTagAlias = await _tagAliasService.GetTagAliasByNameAsync(name, ct);
        if (getTagAlias.IsDefined(out var tagAlias))
        {
            return Result<OneOf<Tag, TagAlias>>.FromSuccess(tagAlias);
        }
        
        return Result<OneOf<Tag, TagAlias>>.FromError(new TagNotFoundError());
    }

    private const string TagNotFoundText = "Tag not found";
}

