using System.Text.Json.Serialization;

namespace RentARide.Application.DTOs.responses.SuperQi;

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
