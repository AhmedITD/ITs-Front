using RentARide.Application.DTOs.Requests.RentalRequest;
using RentARide.Application.DTOs.Responses.Common;
using RentARide.Application.DTOs.Responses.RentalResponse;

namespace RentARide.Application.Interfaces.Services;

public interface IRentalService
{
    Task<ApiResponse<RentalDto>> CreateRental(CreateRentalRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<RentalDto>> CreateRentalFromInvoice(Guid invoiceId, CancellationToken cancellationToken = default);
    Task<ApiResponse<PaginatedListResponse<RentalHistoryItemDto>>> GetMyHistory(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
}
