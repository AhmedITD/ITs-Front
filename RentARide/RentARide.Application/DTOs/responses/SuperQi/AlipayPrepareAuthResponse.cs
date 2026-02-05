using System.Text.Json.Serialization;

namespace RentARide.Application.DTOs.responses.SuperQi;

public class AlipayPrepareAuthResponse
{
    [JsonPropertyName("result")]
    public AlipayResult Result { get; set; } = new();

    [JsonPropertyName("authUrl")]
    public string? AuthUrl { get; set; }
}
