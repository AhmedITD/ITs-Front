using RentARide.Application.DTOs.requests.Invoice;
using RentARide.Application.DTOs.Responses.Common;
using RentARide.Application.DTOs.responses.Invoice;

namespace RentARide.Application.Interfaces.Services;

public interface IInvoiceService
{
    Task<ApiResponse<InvoiceResponse>> CreateInvoice(InvoiceRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> MarkAsPaid(Guid invoiceId, CancellationToken cancellationToken = default);
    Task CancelOtherPendingPaymentsForVehicle(Guid paidInvoiceId, CancellationToken cancellationToken = default);
}