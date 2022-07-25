using Remora.Rest.Core;

namespace TagR.Domain;

public class AuditLog
{
    public Guid Id { get; set; }

    public int TagId { get; set; }

    public Tag? Tag { get; set; }

    public TagAuditLogAction ActionType { get; set; }

    public Snowflake Actor { get; set; }

    public string? Details { get; set; }

    public DateTime TimestampUtc { get; set; }
}