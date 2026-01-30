using RentARide.Application.DTOs.Responses.VehicleResponse;

namespace RentARide.Application.Interfaces.Services;

public interface IVehicleTypeCacheService
{
    Task<IReadOnlyList<VehicleTypeDto>> GetOrSetTypesAsync(Func<CancellationToken, Task<IReadOnlyList<VehicleTypeDto>>> factory, CancellationToken cancellationToken = default);
    void InvalidateTypes();
}
