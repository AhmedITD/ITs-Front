using RentARide.Application.DTOs.Requests.SuperQi;
using RentARide.Application.DTOs.Responses.Common;
using RentARide.Application.DTOs.Responses.SuperQi;

namespace RentARide.Application.Interfaces.Services;

/// <summary>
/// Internal service for SuperQi MiniApp business logic.
/// Port of backend-node/src/api endpoints.
/// </summary>
public interface ISuperQiMiniAppService
{
    /// <summary>Create payment via Alipay+ (for demo/test).</summary>
    Task<ApiResponse<SuperQiPaymentResponse>> CreatePaymentAsync(string userId, CancellationToken ct = default);
    
    /// <summary>Refund a payment via Alipay+.</summary>
    Task<ApiResponse<SuperQiRefundResponse>> RefundPaymentAsync(string paymentId, decimal amount, CancellationToken ct = default);
    
    /// <summary>Send inbox notification to user.</summary>
    Task<ApiResponse<SuperQiNotificationResponse>> SendInboxAsync(string accessToken, string title, string content, string? url, CancellationToken ct = default);
    
    /// <summary>Send push notification to user.</summary>
    Task<ApiResponse<SuperQiNotificationResponse>> SendPushAsync(string accessToken, string title, string content, string? url, CancellationToken ct = default);
    
    /// <summary>Prepare agreement (recurring payment contract).</summary>
    Task<ApiResponse<SuperQiPrepareAuthResponse>> PrepareAgreementAsync(string contractDescription, CancellationToken ct = default);
    
    /// <summary>Exchange auth code for access token (agreement flow).</summary>
    Task<ApiResponse<SuperQiApplyTokenResponse>> ApplyAgreementTokenAsync(string authCode, CancellationToken ct = default);
    
    /// <summary>Execute agreement payment (deduct from wallet automatically).</summary>
    Task<ApiResponse<SuperQiPaymentResponse>> ExecuteAgreementPaymentAsync(
        string accessToken, 
        string customerId, 
        decimal amount, 
        string currency, 
        string orderDescription, 
        CancellationToken ct = default);
}
