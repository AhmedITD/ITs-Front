using System.Text.Json.Serialization;

namespace RentARide.Application.DTOs.responses.SuperQi;

public class AlipayUserInfo
{
    [JsonPropertyName("userId")]
    public string? UserId { get; set; }

    [JsonPropertyName("userName")]
    public string? UserName { get; set; }

    [JsonPropertyName("userAvatar")]
    public string? UserAvatar { get; set; }
}
