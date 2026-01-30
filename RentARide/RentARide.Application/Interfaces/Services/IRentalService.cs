using RentARide.Application.Common;
using RentARide.Application.Common.Pagination;
using RentARide.Application.DTOs.Requests.RentalRequest;
using RentARide.Application.DTOs.Responses.RentalResponse;

namespace RentARide.Application.Interfaces.Services;

public interface IRentalService
{
    Task<ApiResponse<RentalDto>> CreateRental(CreateRentalRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<PaginatedList<RentalHistoryItemDto>>> GetMyHistory(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
}
