namespace RentARide.Application.DTOs.Requests.AuthRequest;

public class LogoutRequest
{
    public required string RefreshToken { get; set; }
}
