using System.ComponentModel.DataAnnotations;

namespace RentARide.Application.DTOs.Requests.AuthRequest;

public class RefreshTokenRequest
{
    [Required]
    public required string RefreshToken { get; set; }
}
