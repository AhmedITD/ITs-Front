using Microsoft.AspNetCore.Identity.Data;
using RentARide.Application.Common;
using RentARide.Application.DTOs.Requests.AuthRequest;
using RentARide.Application.DTOs.Responses.AuthResponse;
using RentARide.Application.DTOs.Responses.Common;
using RentARide.Domain.Entities;
using LoginRequest = RentARide.Application.DTOs.Requests.AuthRequest.LoginRequest;
using RegisterRequest = RentARide.Application.DTOs.Requests.AuthRequest.RegisterRequest;

namespace RentARide.Application.Interfaces.Auth;

public interface IAuthService
{
    Task<ApiResponse<RegisterResponse>> Register(RegisterRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<LoginResponse>> Login(LoginRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<LoginResponse>> Refresh(RefreshTokenRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<LoginResponse>> AuthWithSuperQi(AuthWithSuperQiRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<object>> Logout(LogoutRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<object>> LogoutAllDevices(CancellationToken cancellationToken = default);
    Task<ApiResponse<User>> Me(CancellationToken cancellationToken = default);
}
