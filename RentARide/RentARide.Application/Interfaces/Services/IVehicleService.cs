using RentARide.Application.DTOs.requests.VehicleRequest;
using RentARide.Application.DTOs.responses.Common;
using RentARide.Application.DTOs.responses.VehicleResponse;

namespace RentARide.Application.Interfaces.Services;

public interface IVehicleService
{
    Task<ApiResponse<VehicleDto>> CreateVehicle(CreateVehicleRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<VehicleDto>> UpdatePrice(int vehicleId, UpdateVehiclePriceRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<object>> SoftDeleteVehicle(int vehicleId, CancellationToken cancellationToken = default);
    Task<ApiResponse<IReadOnlyList<VehicleTypeDto>>> GetVehicleTypes(CancellationToken cancellationToken = default);
    Task<ApiResponse<PaginatedListResponse<VehicleDto>>> BrowseVehicles(BrowseVehiclesRequest request, CancellationToken cancellationToken = default);

    Task<ApiResponse<VehicleTypeDto>> CreateVehicleType(CreateVehicleTypeRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<VehicleTypeDto>> UpdateVehicleType(int id, UpdateVehicleTypeRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<object>> DeleteVehicleType(int id, CancellationToken cancellationToken = default);
}
