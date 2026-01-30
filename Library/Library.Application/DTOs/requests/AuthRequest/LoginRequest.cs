using Library.Domain.Enums;

namespace Library.Application.DTOs.requests.AuthRequest;

public class LoginRequest
{
    public required string email { get; set; }
    public required string password { get; set; }
}