using System.Text.Json;
using RentARide.Application.Common;
using RentARide.Application.DTOs.Requests.RentalRequest;
using RentARide.Application.DTOs.Responses.RentalResponse;
using RentARide.Application.Interfaces;
using RentARide.Application.Interfaces.Auth;
using RentARide.Application.Interfaces.Services;
using RentARide.Domain.Entities;
using RentARide.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Mapster;
using RentARide.Application.DTOs.Responses.Common;

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

    public async Task<ApiResponse<RentalDto>> CreateRentalFromInvoice(Guid invoiceId, CancellationToken cancellationToken = default)
    {
        var invoice = await dbContext.Invoices
            .Include(i => i.Vehicle)
            .FirstOrDefaultAsync(i => i.Id == invoiceId, cancellationToken);
        if (invoice == null)
            return ApiResponse<RentalDto>.ErrorResponse("Invoice not found.");
        if (invoice.Status != InvoiceStatus.Paid)
            return ApiResponse<RentalDto>.ErrorResponse("Invoice is not paid.");

        var vehicle = invoice.Vehicle;
        if (vehicle == null)
            return ApiResponse<RentalDto>.ErrorResponse("Vehicle not found.");
        if (vehicle.Status != VehicleStatus.Available)
            return ApiResponse<RentalDto>.ErrorResponse("Vehicle is not available.");

        var startUtc = invoice.StartDate;
        var endUtc = invoice.EndDate;
        var hasOverlap = await dbContext.Rentals
            .AnyAsync(r =>
                r.VehicleId == invoice.VehicleId &&
                r.Status == RentalStatus.Active &&
                startUtc < r.EndDate &&
                endUtc > r.StartDate,
                cancellationToken);
        if (hasOverlap)
            return ApiResponse<RentalDto>.ErrorResponse("Vehicle is already rented for the selected dates.");

        List<int> amenityIds = new();
        if (!string.IsNullOrWhiteSpace(invoice.AmenityIdsJson))
        {
            try
            {
                var parsed = JsonSerializer.Deserialize<List<int>>(invoice.AmenityIdsJson);
                if (parsed != null)
                    amenityIds = parsed.Where(id => id > 0).Distinct().ToList();
            }
            catch { /* ignore invalid json */ }
        }

        var amenities = amenityIds.Count > 0
            ? await dbContext.Amenities
                .Where(a => amenityIds.Contains(a.Id))
                .ToListAsync(cancellationToken)
            : new List<Amenity>();

        var rental = new Rental
        {
            UserId = invoice.UserId,
            VehicleId = invoice.VehicleId,
            StartDate = startUtc,
            EndDate = endUtc,
            TotalPrice = invoice.TotalAmount,
            Status = RentalStatus.Active,
            InvoiceId = invoice.Id,
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

    public async Task<ApiResponse<PaginatedListResponse<RentalHistoryItemDto>>> GetMyHistory(GetMyRentalsRequest request, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Rentals
            .AsNoTracking()
            .Include(r => r.Vehicle)
            .Where(r => r.UserId == currentUser.Id);

        if (request.Status.HasValue)
            query = query.Where(r => r.Status == request.Status.Value);

        if (request.StartDateFrom.HasValue)
        {
            var fromUtc = request.StartDateFrom.Value.ToUtc();
            query = query.Where(r => r.StartDate >= fromUtc);
        }

        if (request.StartDateTo.HasValue)
        {
            var toEndOfDayUtc = request.StartDateTo.Value.Date.AddDays(1).AddTicks(-1).ToUtc();
            query = query.Where(r => r.StartDate <= toEndOfDayUtc);
        }

        if (request.MinPrice.HasValue)
            query = query.Where(r => r.TotalPrice >= request.MinPrice.Value);

        if (request.MaxPrice.HasValue)
            query = query.Where(r => r.TotalPrice <= request.MaxPrice.Value);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.Trim();
            query = query.Where(r =>
                r.Vehicle.Model.Contains(term) ||
                r.Vehicle.LicensePlate.Contains(term));
        }

        query = query.OrderByDescending(r => r.StartDate);

        var projected = query.ProjectToType<RentalHistoryItemDto>();
        var paginated = await PaginatedListResponse<RentalHistoryItemDto>.CreateAsync(
            projected,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        return ApiResponse<PaginatedListResponse<RentalHistoryItemDto>>.SuccessResponse(paginated);
    }
}
