using RentARide.Application.DTOs.Requests.SuperQi;
using RentARide.Application.DTOs.Responses.SuperQi;

namespace RentARide.Application.Interfaces.Services;

/// <summary>
/// External service for calling Alipay+ (SuperQi) APIs.
/// Port of backend-node/src/alipay.
/// </summary>
public interface ISuperQiAlipayService
{
    /// <summary>Exchange auth code for access token (/v1/authorizations/applyToken).</summary>
    Task<AlipayTokenResponse> ApplyTokenAsync(string authCode, CancellationToken ct = default);
    
    /// <summary>Get user info using access token (/v1/users/inquiryUserInfo).</summary>
    Task<AlipayUserInfoResponse> InquiryUserInfoAsync(string accessToken, CancellationToken ct = default);
    
    /// <summary>Get user card list (/v1/users/inquiryUserCardList).</summary>
    Task<AlipayTokenResponse> InquiryUserCardListAsync(string accessToken, CancellationToken ct = default);
    
    /// <summary>Create payment (/v1/payments/pay).</summary>
    Task<AlipayPaymentResponse> PayAsync(AlipayPaymentRequest request, CancellationToken ct = default);
    
    /// <summary>Refund payment (/v1/payments/refund).</summary>
    Task<AlipayRefundResponse> RefundAsync(AlipayRefundRequest request, CancellationToken ct = default);
    
    /// <summary>Inquire refund status (/v1/payments/inquiryRefund).</summary>
    Task<AlipayRefundInquiryResponse> InquiryRefundAsync(string refundRequestId, CancellationToken ct = default);
    
    /// <summary>Send inbox notification (/v1/messages/sendInbox).</summary>
    Task<AlipayNotificationResponse> SendInboxAsync(AlipayNotificationRequest request, CancellationToken ct = default);
    
    /// <summary>Send push notification (/v1/messages/sendPush).</summary>
    Task<AlipayNotificationResponse> SendPushAsync(AlipayNotificationRequest request, CancellationToken ct = default);
    
    /// <summary>Prepare authorization for agreement payment (/v1/authorizations/prepare).</summary>
    Task<AlipayPrepareAuthResponse> PrepareAuthorizationAsync(string contractDescription, CancellationToken ct = default);
}
