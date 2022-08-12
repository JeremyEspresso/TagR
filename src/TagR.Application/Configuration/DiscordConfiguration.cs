using Remora.Rest.Core;

namespace TagR.Application.Configuration;

public sealed record DiscordConfiguration
{
    public string Token { get; init; }

    public string CommandPrefix { get; init; }

    public Snowflake ModeratorRoleId { get; init; }
    
    public Snowflake GuildId { get; init; }
}
