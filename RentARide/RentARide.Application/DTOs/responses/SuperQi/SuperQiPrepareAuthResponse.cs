namespace RentARide.Application.DTOs.responses.SuperQi;

public class SuperQiPrepareAuthResponse
{
    public bool Success { get; set; }
    public string? AuthUrl { get; set; }
    public string? ResultStatus { get; set; }
    public string? ResultCode { get; set; }
    public string? ResultMessage { get; set; }
}
