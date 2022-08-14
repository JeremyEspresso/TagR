using Remora.Commands.Conditions;
using Remora.Discord.Commands.Contexts;
using Remora.Results;
using TagR.Application.ResultErrors;
using TagR.Application.Services.Abstractions;
using TagR.Bot.Commands.Conditions.Attributes;

namespace TagR.Bot.Commands.Conditions;

public class RequireModeratorCondition : ICondition<RequireModeratorAttribute>
{
	private readonly ICommandContext _ctx;
	private readonly IPermissionService _permissionService;

	public RequireModeratorCondition(ICommandContext ctx, IPermissionService permissionService)
	{
		_ctx = ctx;
		_permissionService = permissionService;
	}

	public async ValueTask<Result> CheckAsync(RequireModeratorAttribute attribute, CancellationToken ct = default)
	{
		var isMod = await _permissionService.IsModerator(_ctx.User.ID, ct);

		return isMod
			? Result.FromSuccess()
			: Result.FromError(new InsufficientPermissionsError());

	}
}
