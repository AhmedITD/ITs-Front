namespace RentARide.Application.DTOs.responses.Invoice;

public class InvoiceResponse
{
    public Guid InvoiceId { get; set; }
    public string PaymentUrl { get; set; } = string.Empty;
}