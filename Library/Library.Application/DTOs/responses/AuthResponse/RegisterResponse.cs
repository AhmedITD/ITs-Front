namespace Library.Application.DTOs.responses.AuthResponse;
public class RegisterResponse
{
    public int Id { get; set; }
    public required string name { get; set; }
    public required string email { get; set; }
    public required string token { get; set; }
    public string? refreshToken { get; set; }
}
