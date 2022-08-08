using Remora.Commands.Conditions;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Commands.Contexts;
using Remora.Rest.Core;
using Remora.Results;
using TagR.Application.ResultErrors;
using TagR.Bot.Commands.Conditions.Attributes;

namespace TagR.Bot.Commands.Conditions;

public class RequireModeratorCondition : ICondition<RequireModeratorAttribute>
{
	private readonly ICommandContext _ctx;
	private readonly IDiscordRestGuildAPI _guild;
	private readonly Snowflake _moderatorRoleSnowflake;

	public RequireModeratorCondition(ICommandContext ctx, IDiscordRestGuildAPI guild, IConfig config)
	{
		_ctx = ctx;
		_guild = guild;
		_moderatorRoleSnowflake = new Snowflake(config.Discord.ModeratorRoleId);
	}

	public async ValueTask<Result> CheckAsync(RequireModeratorAttribute attribute, CancellationToken ct = default)
	{
		var user = _ctx.User;
		if (!_ctx.GuildID.IsDefined(out var guildId))
		{
			return Result.FromError(new MessageError("Not in a guild."));
		}

		var getGuildMember = await _guild.GetGuildMemberAsync(guildId, user.ID, ct);
		if (!getGuildMember.IsSuccess)
		{
			return Result.FromError(getGuildMember.Error);
		}

		return getGuildMember.Entity.Roles.Contains(_moderatorRoleSnowflake)
			? Result.FromSuccess()
			: Result.FromError(new MessageError("No moderator role."));
	}
}
