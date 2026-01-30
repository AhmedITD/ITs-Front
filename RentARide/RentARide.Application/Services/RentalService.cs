using RentARide.Application.Common;
using RentARide.Application.Common.Pagination;
using RentARide.Application.DTOs.Requests.RentalRequest;
using RentARide.Application.DTOs.Responses.RentalResponse;
using RentARide.Application.Interfaces;
using RentARide.Application.Interfaces.Auth;
using RentARide.Application.Interfaces.Services;
using RentARide.Domain.Entities;
using RentARide.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Mapster;

namespace RentARide.Application.Services;

public class RentalService(
    IRentARideDbContext dbContext,
    ICurrentUser currentUser,
    IPublicHolidayService publicHolidayService) : IRentalService
{
    public async Task<ApiResponse<RentalDto>> CreateRental(CreateRentalRequest request, CancellationToken cancellationToken = default)
    {
        var vehicle = await dbContext.Vehicles
            .Include(v => v.VehicleType)
            .FirstOrDefaultAsync(v => v.Id == request.VehicleId, cancellationToken);
        if (vehicle == null)
            return ApiResponse<RentalDto>.ErrorResponse("Vehicle not found.");
        if (vehicle.Status != VehicleStatus.Available)
            return ApiResponse<RentalDto>.ErrorResponse("Vehicle is not available.");

        var startUtc = request.StartDate.ToUtc();
        var endUtc = request.EndDate.ToUtc();
        var hasOverlap = await dbContext.Rentals
            .AnyAsync(r =>
                r.VehicleId == request.VehicleId &&
                r.Status == RentalStatus.Active &&
                startUtc < r.EndDate &&
                endUtc > r.StartDate,
                cancellationToken);
        if (hasOverlap)
            return ApiResponse<RentalDto>.ErrorResponse("Vehicle is already rented for the selected dates.");

        var days = (int)Math.Ceiling((endUtc - startUtc).TotalDays);
        if (days < 1) days = 1;

        var validAmenityIds = request.AmenityIds.Where(id => id > 0).Distinct().ToList();
        var amenities = validAmenityIds.Count > 0
            ? await dbContext.Amenities
                .Where(a => validAmenityIds.Contains(a.Id))
                .ToListAsync(cancellationToken)
            : new List<Amenity>();

        if (validAmenityIds.Count > 0 && amenities.Count != validAmenityIds.Count)
        {
            var foundIds = amenities.Select(a => a.Id).ToHashSet();
            var missing = string.Join(", ", validAmenityIds.Where(id => !foundIds.Contains(id)));
            return ApiResponse<RentalDto>.ErrorResponse($"Amenity id(s) not found: {missing}.");
        }

        var amenityPrice = amenities.Sum(a => a.Price);
        var baseTotal = (days * vehicle.DailyPrice) + amenityPrice;
        var holidaySurcharge = 0m;
        if (await publicHolidayService.IsPublicHolidayAsync(startUtc, cancellationToken))
            holidaySurcharge = baseTotal * 0.10m;
        var totalPrice = baseTotal + holidaySurcharge;

        var rental = new Rental
        {
            UserId = currentUser.Id,
            VehicleId = request.VehicleId,
            StartDate = request.StartDate.ToUtc(),
            EndDate = request.EndDate.ToUtc(),
            TotalPrice = totalPrice,
            Status = RentalStatus.Active
        };

        dbContext.Rentals.Add(rental);
        await dbContext.SaveChangesAsync(cancellationToken);

        foreach (var amenity in amenities)
        {
            dbContext.RentalAmenities.Add(new RentalAmenity
            {
                RentalId = rental.Id,
                AmenityId = amenity.Id
            });
        }
        await dbContext.SaveChangesAsync(cancellationToken);

        vehicle.Status = VehicleStatus.Rented;
        await dbContext.SaveChangesAsync(cancellationToken);

        var dto = await dbContext.Rentals
            .AsNoTracking()
            .Include(r => r.Vehicle)
            .Include(r => r.RentalAmenities).ThenInclude(ra => ra.Amenity)
            .Where(r => r.Id == rental.Id)
            .ProjectToType<RentalDto>()
            .FirstOrDefaultAsync(cancellationToken);

        return dto != null
            ? ApiResponse<RentalDto>.SuccessResponse(dto)
            : ApiResponse<RentalDto>.ErrorResponse("Failed to create rental.");
    }

    public async Task<ApiResponse<PaginatedList<RentalHistoryItemDto>>> GetMyHistory(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Rentals
            .AsNoTracking()
            .Include(r => r.Vehicle)
            .Where(r => r.UserId == currentUser.Id)
            .OrderByDescending(r => r.StartDate);

        var projected = query.ProjectToType<RentalHistoryItemDto>();
        var paginated = await PaginatedList<RentalHistoryItemDto>.CreateAsync(projected, pageNumber, pageSize, cancellationToken);

        return ApiResponse<PaginatedList<RentalHistoryItemDto>>.SuccessResponse(paginated);
    }
}
