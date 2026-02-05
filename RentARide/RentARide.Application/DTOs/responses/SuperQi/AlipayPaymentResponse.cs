using System.Text.Json.Serialization;

namespace RentARide.Application.DTOs.responses.SuperQi;

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
