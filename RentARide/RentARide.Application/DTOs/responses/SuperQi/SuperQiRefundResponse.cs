namespace RentARide.Application.DTOs.responses.SuperQi;

public class SuperQiRefundResponse
{
    public bool Success { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? RefundId { get; set; }
    public string? RefundTime { get; set; }
    public string? Message { get; set; }
    public string? ResultStatus { get; set; }
    public string? ResultCode { get; set; }
    public string? ResultMessage { get; set; }
}
