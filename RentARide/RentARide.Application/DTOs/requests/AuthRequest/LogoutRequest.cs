using System.ComponentModel.DataAnnotations;

namespace RentARide.Application.DTOs.Requests.AuthRequest;

public class LogoutRequest
{
    [Required]
    public required string RefreshToken { get; set; }
}
