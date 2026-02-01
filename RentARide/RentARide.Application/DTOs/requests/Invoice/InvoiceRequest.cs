using RentARide.Application.DTOs.Requests.QiCard;
using RentARide.Application.DTOs.Requests.RentalRequest;

namespace RentARide.Application.DTOs.requests.Invoice;

public class InvoiceRequest : CreateRentalRequest
{
    public string FinishUrl { get; set; } = string.Empty;
    /// <summary>Set by the API from HttpContext; not bound from request body.</summary>
    [System.Text.Json.Serialization.JsonIgnore]
    public BrowserInfo? BrowserInfo { get; set; }
}