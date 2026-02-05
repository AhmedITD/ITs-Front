namespace RentARide.Application.DTOs.responses.AuthResponse;

public class LoginResponse
{
    public required string Token { get; set; }
    public required string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiresAt { get; set; }
}
