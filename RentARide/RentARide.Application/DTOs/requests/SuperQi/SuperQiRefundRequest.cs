namespace RentARide.Application.DTOs.requests.SuperQi;

public class SuperQiRefundRequest
{
    public string PaymentId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}
