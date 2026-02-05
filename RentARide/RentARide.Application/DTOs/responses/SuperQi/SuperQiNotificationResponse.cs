namespace RentARide.Application.DTOs.responses.SuperQi;

public class SuperQiNotificationResponse
{
    public bool Success { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? MessageId { get; set; }
    public string? Message { get; set; }
    public string? ResultStatus { get; set; }
    public string? ResultCode { get; set; }
    public string? ResultMessage { get; set; }
}
