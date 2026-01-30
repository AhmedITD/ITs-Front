namespace RentARide.Application.Interfaces.Services;

public interface IPublicHolidayService
{
    Task<bool> IsPublicHolidayAsync(DateTime date, CancellationToken cancellationToken = default);
}
