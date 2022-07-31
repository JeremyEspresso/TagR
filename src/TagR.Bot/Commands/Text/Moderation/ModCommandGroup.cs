using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Objects;
using Remora.Discord.Commands.Contexts;
using Remora.Results;
using TagR.Application.ResultErrors;
using TagR.Application.Services.Abstractions;

namespace TagR.Bot.Commands.Text.Moderation;

[Group("mod")]
public class ModCommandGroup : CommandGroup
{
    private readonly MessageContext _ctx;
    private readonly IModService _modService;
    private readonly IDiscordMessageService _messageService;

    public ModCommandGroup(ICommandContext ctx, IModService modService, IDiscordMessageService messageService)
    {
        _ctx = (MessageContext)ctx;
        _modService = modService;
        _messageService = messageService;
    }

    // Mod only
    [Command("block", "incapacitate")]
    public async Task<IResult> Block(IUser user, [Greedy] string? reason = default)
    {
        var blockUser = await _modService.BlockUserAsync(user.ID, reason, CancellationToken);

        string content;

        if (!blockUser.IsSuccess)
        {
            var userAlreadyBlocked = blockUser.Error as UserIsAlreadyBlockedError;
            var bu = userAlreadyBlocked!.BlockedUser;

            content = $"User `{user.ID}` was already blocked at `{bu.BlockedAtUtc}`. Reason: `{bu.Reason ?? "Not specified."}`";
        }
        else
        {
            content = $"User `{user.ID}` successfully blocked. Reason: `{reason ?? "Not specified."}`";
        }

        await _messageService.CreateMessageAsync
            (
                _ctx.ChannelID,
                content,
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

    // Mod only
    [Command("unblock")]
    public async Task<IResult> Unblock(IUser user)
    {
        var unblockUser = await _modService.UnblockUserAsync(user.ID, CancellationToken);

        await _messageService.CreateMessageAsync
            (
                _ctx.ChannelID,
                unblockUser.IsSuccess ? $"`{user.ID}` unblocked." : unblockUser.Error.Message,
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
}

