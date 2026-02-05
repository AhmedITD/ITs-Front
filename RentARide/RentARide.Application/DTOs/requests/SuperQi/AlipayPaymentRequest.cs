namespace RentARide.Application.DTOs.requests.SuperQi;

public class AlipayPaymentRequest
{
    public string ProductCode { get; set; } = string.Empty;
    public string PaymentRequestId { get; set; } = string.Empty;
    public AlipayAmount PaymentAmount { get; set; } = new();
    public AlipayOrder? Order { get; set; }
    public string? PaymentExpiryTime { get; set; }
    public string? PaymentRedirectUrl { get; set; }
    public string? PaymentNotifyUrl { get; set; }
    public string? PaymentAuthCode { get; set; }
}
