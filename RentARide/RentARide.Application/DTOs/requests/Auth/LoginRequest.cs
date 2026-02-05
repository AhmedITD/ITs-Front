namespace RentARide.Application.DTOs.requests.AuthRequest;

public class LoginRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
