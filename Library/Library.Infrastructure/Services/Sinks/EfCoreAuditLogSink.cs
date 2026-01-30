using Library.Application.Interfaces;
using Library.Application.Interfaces.Sinks;
using Library.Domain.Entities;

namespace Library.Infrastructure.Services.Sinks;

public class EfCoreAuditLogSink(ILibraryDbContext context) : IAuditLogSink
{
    public async Task WriteLogsAsync(IEnumerable<AuditLog> logs, CancellationToken cancellationToken)
    {
        // Since this runs inside "SavingChangesAsync", these will be committed 
        // along with the main transaction automatically and atomically.
        // NOTE!! We shouldn't call SaveChangesAsync here again it will cause a recursion of the interceptor.
        await context.AuditLogs.AddRangeAsync(logs, cancellationToken);
    }
}