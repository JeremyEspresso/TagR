using Microsoft.EntityFrameworkCore;
using Remora.Rest.Core;
using TagR.Database;

namespace TagR.Application.Services;

public class MessageProcessingService : IMessageProcessingService
{
    private readonly TagRDbContext _context;
    private readonly IDiscordMessageService _messageService;
    private readonly ILogger<MessageProcessingService> _logger;

    public MessageProcessingService(TagRDbContext context, IDiscordMessageService messageService, ILogger<MessageProcessingService> logger)
    {
        _context = context;
        _messageService = messageService;
        _logger = logger;
    }

    public async Task ProcessMessageAsync(Snowflake channelId, string messageContent, CancellationToken ct = default)
    {
        var content = StripPrefix(messageContent);

        var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == content, ct);
        if (tag is null)
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

