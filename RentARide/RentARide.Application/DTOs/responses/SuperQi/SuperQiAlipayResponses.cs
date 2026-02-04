using System.Text.Json.Serialization;

namespace RentARide.Application.DTOs.Responses.SuperQi;

/// <summary>Common result structure from Alipay+ API.</summary>
public class AlipayResult
{
    [JsonPropertyName("resultCode")]
    public string ResultCode { get; set; } = string.Empty;
    
    [JsonPropertyName("resultStatus")]
    public string ResultStatus { get; set; } = string.Empty;
    
    [JsonPropertyName("resultMessage")]
    public string? ResultMessage { get; set; }
}

/// <summary>Response from /v1/authorizations/applyToken.</summary>
public class AlipayTokenResponse
{
    [JsonPropertyName("result")]
    public AlipayResult Result { get; set; } = new();
    
    [JsonPropertyName("accessToken")]
    public string? AccessToken { get; set; }
    
    [JsonPropertyName("accessTokenExpiryTime")]
    public string? AccessTokenExpiryTime { get; set; }
    
    [JsonPropertyName("refreshToken")]
    public string? RefreshToken { get; set; }
    
    [JsonPropertyName("refreshTokenExpiryTime")]
    public string? RefreshTokenExpiryTime { get; set; }
    
    [JsonPropertyName("customerId")]
    public string? CustomerId { get; set; }
}

/// <summary>Response from /v1/users/inquiryUserInfo.</summary>
public class AlipayUserInfoResponse
{
    [JsonPropertyName("result")]
    public AlipayResult Result { get; set; } = new();
    
    [JsonPropertyName("userInfo")]
    public AlipayUserInfo? UserInfo { get; set; }
}

public class AlipayUserInfo
{
    [JsonPropertyName("userId")]
    public string? UserId { get; set; }
    
    [JsonPropertyName("userName")]
    public string? UserName { get; set; }
    
    [JsonPropertyName("userAvatar")]
    public string? UserAvatar { get; set; }
}

/// <summary>Response from /v1/payments/pay.</summary>
public class AlipayPaymentResponse
{
    [JsonPropertyName("result")]
    public AlipayResult Result { get; set; } = new();
    
    [JsonPropertyName("paymentId")]
    public string? PaymentId { get; set; }
    
    [JsonPropertyName("paymentRequestId")]
    public string? PaymentRequestId { get; set; }
    
    [JsonPropertyName("paymentTime")]
    public string? PaymentTime { get; set; }
    
    [JsonPropertyName("redirectActionForm")]
    public AlipayRedirectActionForm? RedirectActionForm { get; set; }
}

public class AlipayRedirectActionForm
{
    [JsonPropertyName("redirectUrl")]
    public string? RedirectUrl { get; set; }
}

/// <summary>Response from /v1/payments/refund.</summary>
public class AlipayRefundResponse
{
    [JsonPropertyName("result")]
    public AlipayResult Result { get; set; } = new();
    
    [JsonPropertyName("refundId")]
    public string? RefundId { get; set; }
    
    [JsonPropertyName("refundTime")]
    public string? RefundTime { get; set; }
    
    [JsonPropertyName("refundStatus")]
    public string? RefundStatus { get; set; }
    
    [JsonPropertyName("refundFailReason")]
    public string? RefundFailReason { get; set; }
}

/// <summary>Response from /v1/payments/inquiryRefund.</summary>
public class AlipayRefundInquiryResponse
{
    [JsonPropertyName("result")]
    public AlipayResult Result { get; set; } = new();
    
    [JsonPropertyName("refundId")]
    public string? RefundId { get; set; }
    
    [JsonPropertyName("refundTime")]
    public string? RefundTime { get; set; }
    
    [JsonPropertyName("refundStatus")]
    public string? RefundStatus { get; set; }
    
    [JsonPropertyName("refundFailReason")]
    public string? RefundFailReason { get; set; }
}

/// <summary>Response from /v1/messages/sendInbox or sendPush.</summary>
public class AlipayNotificationResponse
{
    [JsonPropertyName("result")]
    public AlipayResult Result { get; set; } = new();
    
    [JsonPropertyName("messageId")]
    public string? MessageId { get; set; }
    
    [JsonPropertyName("extendInfo")]
    public string? ExtendInfo { get; set; }
}

/// <summary>Response from /v1/authorizations/prepare.</summary>
public class AlipayPrepareAuthResponse
{
    [JsonPropertyName("result")]
    public AlipayResult Result { get; set; } = new();
    
    [JsonPropertyName("authUrl")]
    public string? AuthUrl { get; set; }
}

// ========== SuperQi MiniApp Service Response DTOs ==========

/// <summary>Response for SuperQi payment creation.</summary>
public class SuperQiPaymentResponse
{
    public bool Success { get; set; }
    public string? PaymentUrl { get; set; }
    public string? PaymentId { get; set; }
    public decimal Amount { get; set; }
    public string? Error { get; set; }
}

/// <summary>Response for SuperQi refund.</summary>
public class SuperQiRefundResponse
{
    public bool Success { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? RefundId { get; set; }
    public string? RefundTime { get; set; }
    public string? Message { get; set; }
    public string? ResultStatus { get; set; }
    public string? ResultCode { get; set; }
    public string? ResultMessage { get; set; }
}

/// <summary>Response for SuperQi notification.</summary>
public class SuperQiNotificationResponse
{
    public bool Success { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? MessageId { get; set; }
    public string? Message { get; set; }
    public string? ResultStatus { get; set; }
    public string? ResultCode { get; set; }
    public string? ResultMessage { get; set; }
}

/// <summary>Response for SuperQi agreement preparation.</summary>
public class SuperQiPrepareAuthResponse
{
    public bool Success { get; set; }
    public string? AuthUrl { get; set; }
    public string? ResultStatus { get; set; }
    public string? ResultCode { get; set; }
    public string? ResultMessage { get; set; }
}

/// <summary>Response for SuperQi agreement apply token.</summary>
public class SuperQiApplyTokenResponse
{
    public bool Success { get; set; }
    public string? AccessToken { get; set; }
    public string? CustomerId { get; set; }
    public string? AccessTokenExpiryTime { get; set; }
    public string? ResultStatus { get; set; }
    public string? ResultCode { get; set; }
    public string? ResultMessage { get; set; }
}
