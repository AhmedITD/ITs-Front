using Library.Domain.Entities;

namespace Library.Application.Interfaces.Sinks;

public interface IAuditLogSink
{
    Task WriteLogsAsync(IEnumerable<AuditLog> logs, CancellationToken cancellationToken = default);
}