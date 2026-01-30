using RentARide.Application.Common;
using RentARide.Application.DTOs.Requests.AuthRequest;
using RentARide.Application.DTOs.Responses.AuthResponse;
using RentARide.Domain.Entities;

namespace RentARide.Application.Interfaces.Auth;

public interface IAuthService
{
    Task<ApiResponse<RegisterResponse>> Register(RegisterRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<LoginResponse>> Login(LoginRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<User>> Me(CancellationToken cancellationToken = default);
}
