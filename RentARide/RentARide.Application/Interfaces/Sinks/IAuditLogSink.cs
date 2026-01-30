using RentARide.Domain.Entities;

namespace RentARide.Application.Interfaces.Sinks;

public interface IAuditLogSink
{
    Task WriteLogsAsync(IEnumerable<AuditLog> logs, CancellationToken cancellationToken = default);
}
