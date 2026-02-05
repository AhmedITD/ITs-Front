namespace RentARide.Application.DTOs.responses.SuperQi;

public class SuperQiApplyTokenResponse
{
    public bool Success { get; set; }
    public string? AccessToken { get; set; }
    public string? CustomerId { get; set; }
    public string? AccessTokenExpiryTime { get; set; }
    public string? ResultStatus { get; set; }
    public string? ResultCode { get; set; }
    public string? ResultMessage { get; set; }
}
