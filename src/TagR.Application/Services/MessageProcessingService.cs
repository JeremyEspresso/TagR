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

    public async Task ProcessMessageAsync(Snowflake channelId, Snowflake messageId, string messageContent, CancellationToken ct = default)
    {
        _logger.LogInformation("Start processing message id: {messageId} ", messageId);
        
        var content = StripPrefix(messageContent);

        var getTag = await _tagService.GetTagByName(content);

        var tagDefined = getTag.IsDefined(out var tag);
        await _messageService.CreateMessageAsync(channelId, tagDefined ? tag!.Content : TagNotFoundText, ct);
        
        _logger.LogInformation("End processing message id: {messageId} ", messageId);
    }

    private string StripPrefix(string messageContent)
    {
        return messageContent[1..];
    }

    private const string TagNotFoundText = "Tag not found";
}

