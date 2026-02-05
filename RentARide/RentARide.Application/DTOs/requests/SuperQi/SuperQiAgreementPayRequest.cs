namespace RentARide.Application.DTOs.requests.SuperQi;

public class SuperQiAgreementPayRequest
{
    public string AccessToken { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string? OrderDescription { get; set; }
    public Guid? InvoiceId { get; set; }
}
