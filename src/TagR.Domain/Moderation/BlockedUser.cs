using Remora.Rest.Core;

namespace TagR.Domain.Moderation;

public class BlockedUser
{
    public int Id { get; set; }

    public Snowflake UserSnowflake { get; set; }

    public DateTime BlockedAtUtc { get; set; }

    public BlockedAction BlockedActions { get; set; }
}
