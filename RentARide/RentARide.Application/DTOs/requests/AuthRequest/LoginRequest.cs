namespace RentARide.Application.DTOs.Requests.AuthRequest;

public class LoginRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
