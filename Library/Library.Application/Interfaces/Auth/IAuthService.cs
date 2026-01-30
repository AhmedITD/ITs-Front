using Library.Application.Common;
using Library.Application.DTOs;
using Library.Application.DTOs.requests;
using Library.Application.DTOs.requests.AuthRequest;
using Library.Application.DTOs.responses;
using Library.Application.DTOs.responses.AuthResponse;
using Library.Domain.Entities;

namespace Library.Application.Interfaces.Auth;

public interface IAuthService
{
    Task<ApiResponse<List<User>>> GetAllUsers(CancellationToken cancellationToken = default);
    Task<ApiResponse<RegisterResponse>> Register(RegisterRequest registerRequest, CancellationToken cancellationToken = default);
    Task<ApiResponse<LoginResponse>> Login(LoginRequest loginRequest, CancellationToken cancellationToken = default);
    Task<ApiResponse<User>> Me(CancellationToken cancellationToken = default);
}