using Remora.Rest.Core;

namespace TagR.Domain;

public class Tag
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string Content => Revisions.Last().Content;

    public ICollection<TagRevision> Revisions { get; set; } = new List<TagRevision>();

    public Snowflake OwnerDiscordSnowflake { get; set; }

    public bool Disabled { get; set; }
    
    public bool Locked { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    public ICollection<TagAlias> Aliases { get; set; } = new List<TagAlias>();

    public ICollection<TagUse> Uses { get; set; } = new List<TagUse>();
}
