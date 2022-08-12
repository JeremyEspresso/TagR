using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Rest.Core;
using Remora.Results;
using TagR.Application.ResultErrors;
using TagR.Application.Services.Abstractions;
using TagR.Database;
using TagR.Domain.Moderation;

namespace TagR.Application.Services;

public class PermissionService : IPermissionService
{
    private const double CacheExpirationTime = 5; // 5 minutes;
    private readonly IMemoryCache _cache;
    private readonly IDiscordRestGuildAPI _guildApi;
    private readonly IClock _clock;
    private readonly Snowflake _guildSnowflake;
    private readonly Snowflake _moderatorSnowflake;
    private readonly TagRDbContext _context;

    public PermissionService(IMemoryCache cache, IDiscordRestGuildAPI guildApi, IClock clock, IConfig config, TagRDbContext context)
    {
        _cache = cache;
        _guildApi = guildApi;
        _clock = clock;
        _guildSnowflake = config.Discord.GuildId;
        _moderatorSnowflake = config.Discord.ModeratorRoleId;
        _context = context;
    }

    public async Task<Result> IsActionBlockedAsync(Snowflake actor, BlockedAction blockedActions, CancellationToken ct = default)
    {
        var blockedUser = await _context.BlockedUsers.FirstOrDefaultAsync(bu => bu.UserSnowflake == actor, ct);

        return blockedUser is not null && blockedUser.BlockedActions.HasFlag(blockedActions)
            ? Result.FromError(new MessageError("You are blocked from creating new tags."))
            : Result.FromSuccess();
    }

    public async Task<Result> IsModerator(Snowflake userId, CancellationToken ct = default)
    {
        var member = await _cache.GetOrCreateAsync(userId.ToString(), async (entry) =>
        {
            entry.AbsoluteExpiration = _clock.DateTimeOffsetNow.AddMinutes(CacheExpirationTime);

            var getMember = await _guildApi.GetGuildMemberAsync(_guildSnowflake, userId, ct);
            return getMember.Entity!;
        });

        return member.Roles.Contains(_moderatorSnowflake)
            ? Result.FromSuccess()
            : Result.FromError(new MessageError("Not a moderator."));
    }
}
