using RentARide.Application.Common;
using RentARide.Application.DTOs.Requests.AmenityRequest;
using RentARide.Application.DTOs.Responses.AmenityResponse;
using RentARide.Application.Interfaces;
using RentARide.Application.Interfaces.Services;
using RentARide.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Mapster;
using RentARide.Application.DTOs.Responses.Common;

namespace RentARide.Application.Services;

public class AmenityService(IRentARideDbContext dbContext) : IAmenityService
{
    public async Task<ApiResponse<IReadOnlyList<AmenityDto>>> GetAmenities(CancellationToken cancellationToken = default)
    {
        var list = await dbContext.Amenities
            .AsNoTracking()
            .ProjectToType<AmenityDto>()
            .ToListAsync(cancellationToken);

        return ApiResponse<IReadOnlyList<AmenityDto>>.SuccessResponse(list);
    }

    public async Task<ApiResponse<AmenityDto>> CreateAmenity(CreateAmenityRequest request, CancellationToken cancellationToken = default)
    {
        var amenity = new Amenity
        {
            Name = request.Name,
            Price = request.Price
        };

        dbContext.Amenities.Add(amenity);
        await dbContext.SaveChangesAsync(cancellationToken);

        var dto = await dbContext.Amenities
            .AsNoTracking()
            .Where(a => a.Id == amenity.Id)
            .ProjectToType<AmenityDto>()
            .FirstOrDefaultAsync(cancellationToken);

        return dto != null
            ? ApiResponse<AmenityDto>.SuccessResponse(dto)
            : ApiResponse<AmenityDto>.ErrorResponse("Failed to create amenity.");
    }

    public async Task<ApiResponse<AmenityDto>> UpdateAmenity(int id, UpdateAmenityRequest request, CancellationToken cancellationToken = default)
    {
        var amenity = await dbContext.Amenities.FindAsync([id], cancellationToken);
        if (amenity == null)
            return ApiResponse<AmenityDto>.ErrorResponse("Amenity not found.");

        amenity.Name = request.Name;
        amenity.Price = request.Price;
        await dbContext.SaveChangesAsync(cancellationToken);

        var dto = await dbContext.Amenities
            .AsNoTracking()
            .Where(a => a.Id == id)
            .ProjectToType<AmenityDto>()
            .FirstOrDefaultAsync(cancellationToken);

        return dto != null
            ? ApiResponse<AmenityDto>.SuccessResponse(dto)
            : ApiResponse<AmenityDto>.ErrorResponse("Amenity not found.");
    }

    public async Task<ApiResponse<object>> DeleteAmenity(int id, CancellationToken cancellationToken = default)
    {
        var amenity = await dbContext.Amenities.FindAsync([id], cancellationToken);
        if (amenity == null)
            return ApiResponse<object>.ErrorResponse("Amenity not found.");

        var inUse = await dbContext.RentalAmenities.AnyAsync(ra => ra.AmenityId == id, cancellationToken);
        if (inUse)
            return ApiResponse<object>.ErrorResponse("Cannot delete amenity: one or more rentals use this amenity.");

        dbContext.Amenities.Remove(amenity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ApiResponse<object>.SuccessResponse(new { });
    }
}
