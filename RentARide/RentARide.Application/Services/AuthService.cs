using RentARide.Application.Common;
using RentARide.Application.DTOs.Requests.AuthRequest;
using RentARide.Application.DTOs.Responses.AuthResponse;
using RentARide.Application.Interfaces;
using RentARide.Application.Interfaces.Auth;
using RentARide.Domain.Entities;
using RentARide.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace RentARide.Application.Services;

public class AuthService(
    IRentARideDbContext dbContext,
    IPasswordService passwordService,
    IJwtTokenGenerator tokenGenerator,
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

        var response = new RegisterResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Token = tokenGenerator.GenerateToken(user)
        };

        return ApiResponse<RegisterResponse>.SuccessResponse(response);
    }

    public async Task<ApiResponse<LoginResponse>> Login(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await ValidateUserCredentialsAsync(request.Email, request.Password, cancellationToken);
        if (user == null)
            return ApiResponse<LoginResponse>.ErrorResponse("Invalid credentials.");

        var response = new LoginResponse
        {
            Token = tokenGenerator.GenerateToken(user)
        };

        return ApiResponse<LoginResponse>.SuccessResponse(response);
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
