using TagR.Application.Entities.Auditing;

namespace TagR.Application.Services.Abstractions;

public interface IAuditLogger
{
    Task Log<TEvent>(TEvent auditEvent, CancellationToken ct = default)
                where TEvent : AuditEvent;
}