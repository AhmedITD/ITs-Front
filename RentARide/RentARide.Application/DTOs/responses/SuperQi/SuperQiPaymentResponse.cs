namespace RentARide.Application.DTOs.responses.SuperQi;

public class SuperQiPaymentResponse
{
    public bool Success { get; set; }
    public string? PaymentUrl { get; set; }
    public string? PaymentId { get; set; }
    public decimal Amount { get; set; }
    public string? Error { get; set; }
}
