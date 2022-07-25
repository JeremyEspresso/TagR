using Remora.Rest.Core;
using TagR.Domain;

namespace TagR.Application.Entities.Auditing;

public record TagCreatedEvent(int tagId, Snowflake actor) : AuditEvent(TagAuditLogAction.Create, tagId, actor);

public record TagUpdatedEvent(int tagId, Snowflake actor) : AuditEvent(TagAuditLogAction.Update, tagId, actor);

public record TagDeletedEvent(int tagId, Snowflake actor) : AuditEvent(TagAuditLogAction.Delete, tagId, actor);

public record TagEnabledEvent(int tagId, Snowflake actor) : AuditEvent(TagAuditLogAction.Enable, tagId, actor);

public record TagDisabledEvent(int tagId, Snowflake actor) : AuditEvent(TagAuditLogAction.Disable, tagId, actor);

public record TagAliasedEvent(int tagId, Snowflake actor, int tagAliasId) : AuditEvent(TagAuditLogAction.Alias, tagId, actor);

public record TagCreatedAliasEvent(int tagId, Snowflake actor, int originalTagId) : AuditEvent(TagAuditLogAction.Alias, tagId, actor);