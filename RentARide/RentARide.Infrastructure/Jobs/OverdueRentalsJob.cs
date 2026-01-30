using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RentARide.Application.Interfaces;
using RentARide.Domain.Enums;

namespace RentARide.Infrastructure.Jobs;

public class OverdueRentalsJob(IServiceProvider serviceProvider)
{
    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IRentARideDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<OverdueRentalsJob>>();

        var overdue = await dbContext.Rentals
            .AsNoTracking()
            .Where(r => r.Status == RentalStatus.Active && r.EndDate < DateTime.UtcNow)
            .Select(r => new { r.Id, r.UserId })
            .ToListAsync(cancellationToken);

        foreach (var r in overdue)
            logger.LogWarning("Rental {RentalId} is overdue. User {UserId} has not returned the car.", r.Id, r.UserId);
    }
}
