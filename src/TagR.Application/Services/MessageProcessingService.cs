using Remora.Rest.Core;
using TagR.Application.Services.Abstractions;

namespace TagR.Application.Services;

public class MessageProcessingService : IMessageProcessingService
{
    private readonly ITagService _tagService;
    private readonly IDiscordMessageService _messageService;
    private readonly ILogger<MessageProcessingService> _logger;

    public MessageProcessingService(ITagService tagService, IDiscordMessageService messageService, ILogger<MessageProcessingService> logger)
    {
        _tagService = tagService;
        _messageService = messageService;
        _logger = logger;
    }

    public async Task ProcessMessageAsync(Snowflake channelId, string messageContent, CancellationToken ct = default)
    {
        var content = StripPrefix(messageContent);

        var getTag = await _tagService.GetTagByName(content);
        if (!getTag.IsDefined(out var tag))
        {
            await _messageService.CreateMessageAsync(channelId, "Tag not found", ct);
            return;
        }

        await _messageService.CreateMessageAsync(channelId, tag.Content, ct);
    }

    private string StripPrefix(string messageContent)
    {
        return messageContent[1..];
    }
}

