namespace RentARide.Application.DTOs.Responses.AuthResponse;

public class RegisterResponse
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Token { get; set; }
}
