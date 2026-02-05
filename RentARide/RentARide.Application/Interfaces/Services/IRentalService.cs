using RentARide.Application.DTOs.requests.RentalRequest;
using RentARide.Application.DTOs.responses.Common;
using RentARide.Application.DTOs.responses.RentalResponse;

namespace RentARide.Application.Interfaces.Services;

public interface IRentalService
{
    Task<ApiResponse<RentalDto>> CreateRental(CreateRentalRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<RentalDto>> CreateRentalFromInvoice(Guid invoiceId, CancellationToken cancellationToken = default);
    Task<ApiResponse<PaginatedListResponse<RentalHistoryItemDto>>> GetMyHistory(GetMyRentalsRequest request, CancellationToken cancellationToken = default);
}
