namespace TagR.Application.Configuration;

public sealed record DiscordConfiguration
{
    public string Token { get; init; }

    public string CommandPrefix { get; init; }

    public ulong ModeratorRoleId { get; init; }
}
