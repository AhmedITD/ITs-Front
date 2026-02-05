using System.Text.Json.Serialization;

namespace RentARide.Application.DTOs.responses.SuperQi;

public class AlipayNotificationResponse
{
    [JsonPropertyName("result")]
    public AlipayResult Result { get; set; } = new();

    [JsonPropertyName("messageId")]
    public string? MessageId { get; set; }

    [JsonPropertyName("extendInfo")]
    public string? ExtendInfo { get; set; }
}
