using RentARide.Application.DTOs.requests.SuperQi;
using RentARide.Application.DTOs.responses.Common;
using RentARide.Application.DTOs.responses.SuperQi;

namespace RentARide.Application.Interfaces.Services;

public interface ISuperQiMiniAppService
{
    Task<ApiResponse<SuperQiPaymentResponse>> CreatePaymentAsync(string userId, CancellationToken ct = default);
    
    Task<ApiResponse<SuperQiRefundResponse>> RefundPaymentAsync(string paymentId, decimal amount, CancellationToken ct = default);
    
    Task<ApiResponse<SuperQiNotificationResponse>> SendInboxAsync(string accessToken, string title, string content, string? url, CancellationToken ct = default);
    
    Task<ApiResponse<SuperQiNotificationResponse>> SendPushAsync(string accessToken, string title, string content, string? url, CancellationToken ct = default);
    
    Task<ApiResponse<SuperQiPrepareAuthResponse>> PrepareAgreementAsync(string contractDescription, CancellationToken ct = default);
    
    Task<ApiResponse<SuperQiApplyTokenResponse>> ApplyAgreementTokenAsync(string authCode, CancellationToken ct = default);
    
    Task<ApiResponse<SuperQiPaymentResponse>> ExecuteAgreementPaymentAsync(SuperQiAgreementPayRequest request, CancellationToken ct = default);

    /// <summary>Creates an online purchase payment for an invoice. Links paymentRequestId to invoice for webhook correlation.</summary>
    Task<ApiResponse<SuperQiPaymentResponse>> CreatePaymentForInvoiceAsync(Guid invoiceId, string finishPaymentUrl, CancellationToken ct = default);
}
