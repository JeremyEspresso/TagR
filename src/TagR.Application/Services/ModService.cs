using Microsoft.EntityFrameworkCore;
using Remora.Rest.Core;
using Remora.Results;
using TagR.Application.ResultErrors;
using TagR.Application.Services.Abstractions;
using TagR.Database;
using TagR.Domain.Moderation;

namespace TagR.Application.Services;

public class ModService : IModService
{
    private readonly TagRDbContext _context;
    private readonly IClock _clock;

    public ModService(TagRDbContext context, IClock clock)
    {
        _context = context;
        _clock = clock;
    }

    public async Task<IResult> BlockUserAsync(Snowflake userId, string? reason, CancellationToken ct = default)
    {
        var blockedUser = await GetBlockedUserBySnowflakeAsync(userId, ct);

        if (blockedUser.IsDefined(out var bu))
        {
            return Result.FromError(new UserIsAlreadyBlockedError(bu));
        }

        _context.BlockedUsers.Add(new BlockedUser
        {
            UserSnowflake = userId,
            Reason = reason,
            BlockedAtUtc = _clock.UtcNow
        });
        await _context.SaveChangesAsync(ct);

        return Result.FromSuccess();
    }

    public async Task<IResult> UnblockUserAsync(Snowflake userId, CancellationToken ct = default)
    {
        var blockedUser = await GetBlockedUserBySnowflakeAsync(userId, ct);

        if (!blockedUser.IsDefined(out var bu))
        {
            return Result.FromError(new UserIsNotBlockedError(userId));
        }

        _context.BlockedUsers.Remove(bu);
        await _context.SaveChangesAsync(ct);

        return Result.FromSuccess();
    }

    public async Task<Optional<BlockedUser>> GetBlockedUserBySnowflakeAsync(Snowflake userId, CancellationToken ct = default)
    {
        return (await _context.BlockedUsers.FirstOrDefaultAsync(bu => bu.UserSnowflake == userId, ct))!;
    }
}
