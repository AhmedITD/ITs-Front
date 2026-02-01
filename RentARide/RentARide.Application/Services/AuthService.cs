using RentARide.Application.Common;
using RentARide.Application.DTOs.Requests.AuthRequest;
using RentARide.Application.DTOs.Responses.AuthResponse;
using RentARide.Application.Interfaces;
using RentARide.Application.Interfaces.Auth;
using RentARide.Domain.Entities;
using RentARide.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using RentARide.Application.DTOs.Responses.Common;

namespace RentARide.Application.Services;

public class AuthService(
    IRentARideDbContext dbContext,
    IPasswordService passwordService,
    IJwtTokenGenerator tokenGenerator,
    IRefreshTokenService refreshTokenService,
    ICurrentUser currentUser) : IAuthService
{
    public async Task<ApiResponse<RegisterResponse>> Register(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        if (await dbContext.Users.AnyAsync(u => u.Email == normalizedEmail, cancellationToken))
            return ApiResponse<RegisterResponse>.ErrorResponse("Email already exists.");

        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = normalizedEmail,
            PasswordHash = passwordService.HashPassword(request.Password),
            Role = UserRole.Customer
        };

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        var (refreshToken, refreshExpiresAt) = await refreshTokenService.CreateAsync(user.Id, cancellationToken: cancellationToken);
        var response = new RegisterResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Token = tokenGenerator.GenerateToken(user),
            RefreshToken = refreshToken,
            RefreshTokenExpiresAt = refreshExpiresAt
        };

        return ApiResponse<RegisterResponse>.SuccessResponse(response);
    }

    public async Task<ApiResponse<LoginResponse>> Login(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await ValidateUserCredentialsAsync(request.Email, request.Password, cancellationToken);
        if (user == null)
            return ApiResponse<LoginResponse>.ErrorResponse("Invalid credentials.");

        var (refreshToken, refreshExpiresAt) = await refreshTokenService.CreateAsync(user.Id, cancellationToken: cancellationToken);
        var response = new LoginResponse
        {
            Token = tokenGenerator.GenerateToken(user),
            RefreshToken = refreshToken,
            RefreshTokenExpiresAt = refreshExpiresAt
        };

        return ApiResponse<LoginResponse>.SuccessResponse(response);
    }

    public async Task<ApiResponse<LoginResponse>> Refresh(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var user = await refreshTokenService.ValidateAsync(request.RefreshToken, cancellationToken);
        if (user == null)
            return ApiResponse<LoginResponse>.ErrorResponse("Invalid or expired refresh token.");

        await refreshTokenService.RevokeAsync(request.RefreshToken, cancellationToken);
        var (newRefreshToken, refreshExpiresAt) = await refreshTokenService.CreateAsync(user.Id, cancellationToken: cancellationToken);
        var response = new LoginResponse
        {
            Token = tokenGenerator.GenerateToken(user),
            RefreshToken = newRefreshToken,
            RefreshTokenExpiresAt = refreshExpiresAt
        };
        return ApiResponse<LoginResponse>.SuccessResponse(response);
    }

    public async Task<ApiResponse<object>> Logout(LogoutRequest request, CancellationToken cancellationToken = default)
    {
        await refreshTokenService.RevokeAsync(request.RefreshToken, cancellationToken);
        return ApiResponse<object>.SuccessResponse(new { }, "Logged out successfully.");
    }
    public async Task<ApiResponse<object>> LogoutAllDevices(CancellationToken cancellationToken = default)
    {
        await refreshTokenService.RevokeAllForUserAsync(currentUser.Id, cancellationToken);
        return ApiResponse<object>.SuccessResponse(new { }, "Logged out from all devices.");
    }
    

    public async Task<ApiResponse<User>> Me(CancellationToken cancellationToken = default)
    {
        var user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == currentUser.Id, cancellationToken);
        if (user == null)
            return ApiResponse<User>.ErrorResponse("User not found.");
        return ApiResponse<User>.SuccessResponse(user);
    }

    private async Task<User?> ValidateUserCredentialsAsync(string email, string password, CancellationToken cancellationToken)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();
        var user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == normalizedEmail, cancellationToken);
        if (user == null)
            return null;
        return passwordService.VerifyPassword(password, user.PasswordHash) ? user : null;
    }
}
