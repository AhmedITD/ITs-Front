using System.Text.Json.Serialization;

namespace RentARide.Application.DTOs.responses.SuperQi;

public class AlipayRedirectActionForm
{
    [JsonPropertyName("redirectUrl")]
    public string? RedirectUrl { get; set; }
}
