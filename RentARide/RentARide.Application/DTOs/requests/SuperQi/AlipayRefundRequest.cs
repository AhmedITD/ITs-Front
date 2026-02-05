namespace RentARide.Application.DTOs.requests.SuperQi;

public class AlipayRefundRequest
{
    public string RefundRequestId { get; set; } = string.Empty;
    public string PaymentId { get; set; } = string.Empty;
    public AlipayAmount RefundAmount { get; set; } = new();
    public string? RefundReason { get; set; }
}
