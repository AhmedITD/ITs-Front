namespace RentARide.Application.DTOs.requests.SuperQi;

public class SuperQiNotificationRequest
{
    public string AccessToken { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Url { get; set; }
}
