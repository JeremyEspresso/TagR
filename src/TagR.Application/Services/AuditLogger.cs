using TagR.Application.Entities.Auditing;
using TagR.Application.Services.Abstractions;
using TagR.Database;
using TagR.Domain;

namespace TagR.Application.Services;

public class AuditLogger : IAuditLogger
{
    private readonly TagRDbContext _context;
    private readonly IClock _clock;

    public AuditLogger(TagRDbContext context, IClock clock)
    {
        _context = context;
        _clock = clock;
    }

    public Task Log<TEvent>(TEvent auditEvent, CancellationToken ct = default)
        where TEvent : AuditEvent
            => auditEvent switch
            {
                TagAliasedEvent tae => Log(tae),
                TagCreatedAliasEvent tcae => Log(tcae),
                _ => InsertSimpleAuditLog(auditEvent, ct)
            };

    private Task Log(TagAliasedEvent auditEvent, CancellationToken ct = default)
    {
        var auditLog = new AuditLog
        {
            TimestampUtc = _clock.UtcNow,
            TagId = auditEvent.TagId,
            ActionType = auditEvent.AuditAction,
            Actor = auditEvent.Actor,
            Details = $"Tag alias ID: {auditEvent.tagAliasId}"
        };

        _context.AuditLogs.Add(auditLog);
        return _context.SaveChangesAsync(ct);
    }

    private Task Log(TagCreatedAliasEvent auditEvent, CancellationToken ct = default)
    {
        var auditLog = new AuditLog
        {
            TimestampUtc = _clock.UtcNow,
            TagId = auditEvent.TagId,
            ActionType = auditEvent.AuditAction,
            Actor = auditEvent.Actor,
            Details = $"Orginal tag ID: {auditEvent.originalTagId}"
        };

        _context.AuditLogs.Add(auditLog);
        return _context.SaveChangesAsync(ct);
    }

    private Task<int> InsertSimpleAuditLog(AuditEvent auditEvent, CancellationToken ct)
    {
        var auditLog = new AuditLog
        {
            TimestampUtc = _clock.UtcNow,
            TagId = auditEvent.TagId,
            ActionType = auditEvent.AuditAction,
            Actor = auditEvent.Actor
        };

        _context.AuditLogs.Add(auditLog);
        return _context.SaveChangesAsync(ct);
    }
}

