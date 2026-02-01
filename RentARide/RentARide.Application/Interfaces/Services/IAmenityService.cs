using RentARide.Application.Common;
using RentARide.Application.DTOs.Requests.AmenityRequest;
using RentARide.Application.DTOs.Responses.AmenityResponse;
using RentARide.Application.DTOs.Responses.Common;

namespace RentARide.Application.Interfaces.Services;

public interface IAmenityService
{
    Task<ApiResponse<IReadOnlyList<AmenityDto>>> GetAmenities(CancellationToken cancellationToken = default);
    Task<ApiResponse<AmenityDto>> CreateAmenity(CreateAmenityRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<AmenityDto>> UpdateAmenity(int id, UpdateAmenityRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<object>> DeleteAmenity(int id, CancellationToken cancellationToken = default);
}
