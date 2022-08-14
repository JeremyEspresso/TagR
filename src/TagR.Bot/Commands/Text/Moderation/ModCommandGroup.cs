using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Objects;
using Remora.Discord.Commands.Contexts;
using Remora.Results;
using TagR.Application.ResultErrors;
using TagR.Application.Services.Abstractions;
using TagR.Bot.Commands.Conditions.Attributes;
using TagR.Domain.Moderation;

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

    [RequireModerator]
    [Command("block", "incapacitate")]
    public async Task<IResult> Block(IUser user, BlockedAction actions)
    {
        var blockUser = await _modService.BlockUserAsync(user.ID, actions, _ctx.User.ID, CancellationToken);

        var content = string.Empty;

        if (!blockUser.IsSuccess)
        {
	        switch (blockUser.Error)
	        {
		        case UserIsAlreadyBlockedForActionsError userAlreadyBlocked:
		        {
			        var bu = userAlreadyBlocked!.BlockedUser;
			        content = $"User `{user.ID}` was already blocked at `{bu.BlockedAtUtc}`.";
			        break;
		        }
		        case UnableToBlockSelfError ube:
			        content = ube.Message;
			        break;
	        }
        }
        else
        {
            content = $"User `{user.ID}` successfully blocked.";
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

    [RequireModerator]
    [Command("unblock")]
    public async Task<IResult> Unblock(IUser user, BlockedAction actions)
    {
        var unblockUser = await _modService.UnblockUserAsync(user.ID, actions, _ctx.User.ID, CancellationToken);

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

