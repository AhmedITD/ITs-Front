using System.Text.Json.Serialization;

namespace RentARide.Application.DTOs.responses.SuperQi;

public class AlipayResult
{
    [JsonPropertyName("resultCode")]
    public string ResultCode { get; set; } = string.Empty;

    [JsonPropertyName("resultStatus")]
    public string ResultStatus { get; set; } = string.Empty;

    [JsonPropertyName("resultMessage")]
    public string? ResultMessage { get; set; }
}
