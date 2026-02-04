namespace RentARide.Application.DTOs.Requests.SuperQi;

/// <summary>Payment request for Alipay+ API.</summary>
public class AlipayPaymentRequest
{
    public string ProductCode { get; set; } = string.Empty;
    public string PaymentRequestId { get; set; } = string.Empty;
    public AlipayAmount PaymentAmount { get; set; } = new();
    public AlipayOrder? Order { get; set; }
    public string? PaymentExpiryTime { get; set; }
    public string? PaymentRedirectUrl { get; set; }
    public string? PaymentNotifyUrl { get; set; }
    /// <summary>For agreement payment: the access token from authorization.</summary>
    public string? PaymentAuthCode { get; set; }
}

public class AlipayAmount
{
    public string Currency { get; set; } = "IQD";
    public string Value { get; set; } = "0";
}

public class AlipayOrder
{
    public string? OrderDescription { get; set; }
    public AlipayBuyer? Buyer { get; set; }
}

public class AlipayBuyer
{
    public string? ReferenceBuyerId { get; set; }
}

/// <summary>Refund request for Alipay+ API.</summary>
public class AlipayRefundRequest
{
    public string RefundRequestId { get; set; } = string.Empty;
    public string PaymentId { get; set; } = string.Empty;
    public AlipayAmount RefundAmount { get; set; } = new();
    public string? RefundReason { get; set; }
}

/// <summary>Notification request for Alipay+ API (inbox/push).</summary>
public class AlipayNotificationRequest
{
    public string AccessToken { get; set; } = string.Empty;
    public string RequestId { get; set; } = string.Empty;
    public string TemplateCode { get; set; } = string.Empty;
    public List<AlipayNotificationTemplate> Templates { get; set; } = new();
}

public class AlipayNotificationTemplate
{
    public Dictionary<string, string> TemplateParameters { get; set; } = new();
}

/// <summary>Request DTOs for SuperQi MiniApp endpoints.</summary>
public class SuperQiCreatePaymentRequest
{
    // User is determined from JWT; no extra fields needed for demo
}

public class SuperQiRefundRequest
{
    public string PaymentId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

public class SuperQiNotificationRequest
{
    public string AccessToken { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Url { get; set; }
}

public class SuperQiPrepareAgreementRequest
{
    public string ContractDescription { get; set; } = string.Empty;
}

public class SuperQiApplyTokenRequest
{
    public string AuthCode { get; set; } = string.Empty;
}

public class SuperQiAgreementPayRequest
{
    public string AccessToken { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "IQD";
    public string? OrderDescription { get; set; }
}
