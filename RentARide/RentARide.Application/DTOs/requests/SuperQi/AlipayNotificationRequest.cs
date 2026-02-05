namespace RentARide.Application.DTOs.requests.SuperQi;

public class AlipayNotificationRequest
{
    public string AccessToken { get; set; } = string.Empty;
    public string RequestId { get; set; } = string.Empty;
    public string TemplateCode { get; set; } = string.Empty;
    public List<AlipayNotificationTemplate> Templates { get; set; } = new();
}
