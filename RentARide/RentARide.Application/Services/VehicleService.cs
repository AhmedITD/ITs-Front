using RentARide.Application.Common;
using RentARide.Application.DTOs.Requests.VehicleRequest;
using RentARide.Application.DTOs.Responses.VehicleResponse;
using RentARide.Application.Interfaces;
using RentARide.Application.Interfaces.Services;
using RentARide.Domain.Entities;
using RentARide.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Mapster;
using RentARide.Application.DTOs.Responses.Common;

namespace RentARide.Application.Services;

public class VehicleService(
    IRentARideDbContext dbContext,
    IVehicleTypeCacheService vehicleTypeCacheService) : IVehicleService
{
    private const string VehicleTypesCacheKey = "VehicleTypes";

    public async Task<ApiResponse<VehicleDto>> CreateVehicle(CreateVehicleRequest request, CancellationToken cancellationToken = default)
    {
        var vehicleType = await dbContext.VehicleTypes.FindAsync([request.VehicleTypeId], cancellationToken);
        if (vehicleType == null)
            return ApiResponse<VehicleDto>.ErrorResponse("Vehicle type not found.");

        var vehicle = new Vehicle
        {
            Model = request.Model,
            Year = request.Year,
            LicensePlate = request.LicensePlate,
            DailyPrice = request.DailyPrice,
            Status = VehicleStatus.Available,
            VehicleTypeId = request.VehicleTypeId
        };

        dbContext.Vehicles.Add(vehicle);
        await dbContext.SaveChangesAsync(cancellationToken);

        if (!string.IsNullOrEmpty(request.MaintenanceDescription) || request.LastMaintenanceDate.HasValue || request.NextMaintenanceDue.HasValue)
        {
            var maintenance = new VehicleMaintenance
            {
                VehicleId = vehicle.Id,
                Description = request.MaintenanceDescription,
                LastMaintenanceDate = request.LastMaintenanceDate.ToUtc(),
                NextMaintenanceDue = request.NextMaintenanceDue.ToUtc()
            };
            dbContext.VehicleMaintenances.Add(maintenance);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        var dto = await dbContext.Vehicles
            .AsNoTracking()
            .Include(v => v.VehicleType)
            .Where(v => v.Id == vehicle.Id)
            .ProjectToType<VehicleDto>()
            .FirstOrDefaultAsync(cancellationToken);

        return dto != null
            ? ApiResponse<VehicleDto>.SuccessResponse(dto)
            : ApiResponse<VehicleDto>.ErrorResponse("Failed to create vehicle.");
    }

    public async Task<ApiResponse<VehicleDto>> UpdatePrice(int vehicleId, UpdateVehiclePriceRequest request, CancellationToken cancellationToken = default)
    {
        var vehicle = await dbContext.Vehicles.FindAsync([vehicleId], cancellationToken);
        if (vehicle == null)
            return ApiResponse<VehicleDto>.ErrorResponse("Vehicle not found.");

        vehicle.DailyPrice = request.DailyPrice;
        await dbContext.SaveChangesAsync(cancellationToken);

        var dto = await dbContext.Vehicles
            .AsNoTracking()
            .Include(v => v.VehicleType)
            .Where(v => v.Id == vehicleId)
            .ProjectToType<VehicleDto>()
            .FirstOrDefaultAsync(cancellationToken);

        return dto != null
            ? ApiResponse<VehicleDto>.SuccessResponse(dto)
            : ApiResponse<VehicleDto>.ErrorResponse("Vehicle not found.");
    }

    public async Task<ApiResponse<object>> SoftDeleteVehicle(int vehicleId, CancellationToken cancellationToken = default)
    {
        var vehicle = await dbContext.Vehicles.FindAsync([vehicleId], cancellationToken);
        if (vehicle == null)
            return ApiResponse<object>.ErrorResponse("Vehicle not found.");

        dbContext.Vehicles.Remove(vehicle);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ApiResponse<object>.SuccessResponse(new { });
    }

    public async Task<ApiResponse<IReadOnlyList<VehicleTypeDto>>> GetVehicleTypes(CancellationToken cancellationToken = default)
    {
        var types = await vehicleTypeCacheService.GetOrSetTypesAsync(async ct =>
        {
            return await dbContext.VehicleTypes
                .AsNoTracking()
                .ProjectToType<VehicleTypeDto>()
                .ToListAsync(ct);
        }, cancellationToken);

        return ApiResponse<IReadOnlyList<VehicleTypeDto>>.SuccessResponse(types);
    }

    public async Task<ApiResponse<PaginatedListResponse<VehicleDto>>> BrowseVehicles(BrowseVehiclesRequest request, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Vehicles
            .AsNoTracking()
            .Include(v => v.VehicleType)
            .AsQueryable();

        if (request.VehicleTypeId.HasValue)
            query = query.Where(v => v.VehicleTypeId == request.VehicleTypeId.Value);

        var projected = query.ProjectToType<VehicleDto>();
        var paginated = await PaginatedListResponse<VehicleDto>.CreateAsync(
            projected,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        return ApiResponse<PaginatedListResponse<VehicleDto>>.SuccessResponse(paginated);
    }

    public async Task<ApiResponse<VehicleTypeDto>> CreateVehicleType(CreateVehicleTypeRequest request, CancellationToken cancellationToken = default)
    {
        var vehicleType = new VehicleType
        {
            Name = request.Name,
            Description = request.Description
        };

        dbContext.VehicleTypes.Add(vehicleType);
        await dbContext.SaveChangesAsync(cancellationToken);

        vehicleTypeCacheService.InvalidateTypes();

        var dto = await dbContext.VehicleTypes
            .AsNoTracking()
            .Where(t => t.Id == vehicleType.Id)
            .ProjectToType<VehicleTypeDto>()
            .FirstOrDefaultAsync(cancellationToken);

        return dto != null
            ? ApiResponse<VehicleTypeDto>.SuccessResponse(dto)
            : ApiResponse<VehicleTypeDto>.ErrorResponse("Failed to create vehicle type.");
    }

    public async Task<ApiResponse<VehicleTypeDto>> UpdateVehicleType(int id, UpdateVehicleTypeRequest request, CancellationToken cancellationToken = default)
    {
        var vehicleType = await dbContext.VehicleTypes.FindAsync([id], cancellationToken);
        if (vehicleType == null)
            return ApiResponse<VehicleTypeDto>.ErrorResponse("Vehicle type not found.");

        vehicleType.Name = request.Name;
        vehicleType.Description = request.Description;
        await dbContext.SaveChangesAsync(cancellationToken);

        vehicleTypeCacheService.InvalidateTypes();

        var dto = await dbContext.VehicleTypes
            .AsNoTracking()
            .Where(t => t.Id == id)
            .ProjectToType<VehicleTypeDto>()
            .FirstOrDefaultAsync(cancellationToken);

        return dto != null
            ? ApiResponse<VehicleTypeDto>.SuccessResponse(dto)
            : ApiResponse<VehicleTypeDto>.ErrorResponse("Vehicle type not found.");
    }

    public async Task<ApiResponse<object>> DeleteVehicleType(int id, CancellationToken cancellationToken = default)
    {
        var vehicleType = await dbContext.VehicleTypes.FindAsync([id], cancellationToken);
        if (vehicleType == null)
            return ApiResponse<object>.ErrorResponse("Vehicle type not found.");

        var hasVehicles = await dbContext.Vehicles.AnyAsync(v => v.VehicleTypeId == id, cancellationToken);
        if (hasVehicles)
            return ApiResponse<object>.ErrorResponse("Cannot delete vehicle type: one or more vehicles use this type.");

        dbContext.VehicleTypes.Remove(vehicleType);
        await dbContext.SaveChangesAsync(cancellationToken);

        vehicleTypeCacheService.InvalidateTypes();

        return ApiResponse<object>.SuccessResponse(new { });
    }
}
