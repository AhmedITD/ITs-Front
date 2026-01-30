using Library.Domain.Enums;

namespace Library.Application.DTOs.requests.AuthRequest;
public class RegisterRequest
{
    public required string name { get; set; }
    public required string email { get; set; }
    public required string password { get; set; }
    public UserRole role { get; set; }
}