using System.ComponentModel.DataAnnotations;

namespace RentARide.Application.DTOs.requests.AuthRequest;

public class RefreshTokenRequest
{
    [Required]
    public required string RefreshToken { get; set; }
}
