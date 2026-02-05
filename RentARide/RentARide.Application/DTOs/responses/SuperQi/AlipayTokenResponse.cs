using System.Text.Json.Serialization;

namespace RentARide.Application.DTOs.responses.SuperQi;

public class AlipayTokenResponse
{
    [JsonPropertyName("result")]
    public AlipayResult Result { get; set; } = new();

    [JsonPropertyName("accessToken")]
    public string? AccessToken { get; set; }

    [JsonPropertyName("accessTokenExpiryTime")]
    public string? AccessTokenExpiryTime { get; set; }

    [JsonPropertyName("refreshToken")]
    public string? RefreshToken { get; set; }

    [JsonPropertyName("refreshTokenExpiryTime")]
    public string? RefreshTokenExpiryTime { get; set; }

    [JsonPropertyName("customerId")]
    public string? CustomerId { get; set; }
}
