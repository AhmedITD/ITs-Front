using RentARide.Application.Interfaces;
using RentARide.Application.Interfaces.Sinks;
using RentARide.Domain.Entities;

namespace RentARide.Infrastructure.Services.Sinks;

public class EfCoreAuditLogSink(IRentARideDbContext context) : IAuditLogSink
{
    public async Task WriteLogsAsync(IEnumerable<AuditLog> logs, CancellationToken cancellationToken = default)
    {
        await context.AuditLogs.AddRangeAsync(logs, cancellationToken);
    }
}
