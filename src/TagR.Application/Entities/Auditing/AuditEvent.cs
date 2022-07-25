using Remora.Rest.Core;
using TagR.Domain;

namespace TagR.Application.Entities.Auditing;

public record AuditEvent
{
    public TagAuditLogAction AuditAction { get; }

    public int TagId { get; }

    public Snowflake Actor { get; }

    public AuditEvent(TagAuditLogAction auditAction, int tagId, Snowflake actor)
    {
        AuditAction = auditAction;
        TagId = tagId;
        Actor = actor;
    }
}