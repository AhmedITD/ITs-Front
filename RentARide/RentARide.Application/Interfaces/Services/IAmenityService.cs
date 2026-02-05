using RentARide.Application.Common;
using RentARide.Application.DTOs.requests.AmenityRequest;
using RentARide.Application.DTOs.responses.AmenityResponse;
using RentARide.Application.DTOs.responses.Common;

namespace RentARide.Application.Interfaces.Services;

public interface IAmenityService
{
    Task<ApiResponse<IReadOnlyList<AmenityDto>>> GetAmenities(CancellationToken cancellationToken = default);
    Task<ApiResponse<AmenityDto>> CreateAmenity(CreateAmenityRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<AmenityDto>> UpdateAmenity(int id, UpdateAmenityRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<object>> DeleteAmenity(int id, CancellationToken cancellationToken = default);
}
