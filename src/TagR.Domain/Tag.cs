using Remora.Rest.Core;

namespace TagR.Domain;

public class Tag
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string Content => Revisions.Last().Content;

    public ICollection<TagRevision> Revisions { get; set; } = default!;

    public Snowflake OwnerDiscordSnowflake { get; set; }

    public int Uses { get; set; }

    public bool Disabled { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public ICollection<AuditLog>? AuditLogs { get; set; }
}
