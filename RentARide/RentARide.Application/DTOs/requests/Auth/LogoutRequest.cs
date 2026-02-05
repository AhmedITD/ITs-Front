using System.ComponentModel.DataAnnotations;

namespace RentARide.Application.DTOs.requests.AuthRequest;

public class LogoutRequest
{
    [Required]
    public required string RefreshToken { get; set; }
}
