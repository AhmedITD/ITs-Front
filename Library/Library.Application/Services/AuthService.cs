using Library.Application.Common;
using Library.Application.DTOs.requests;
using Library.Application.DTOs.responses;
using Library.Application.DTOs;
using Library.Application.DTOs.requests.AuthRequest;
using Library.Application.DTOs.responses.AuthResponse;
using Library.Application.Interfaces;
using Library.Application.Interfaces.Auth;
using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace Library.Application.Services;

public class AuthService(
    ILibraryDbContext DbContext,
    IPasswordService hasher,
    IJwtTokenGenerator tokenGenerator,
    ICurrentUser CurrentUser
    ) : IAuthService
{
    public async Task<ApiResponse<List<User>>> GetAllUsers(CancellationToken cancellationToken = default)
    {
        return ApiResponse<List<User>>.SuccessResponse(await DbContext.Users.ToListAsync(cancellationToken));
    }

    public async Task<ApiResponse<RegisterResponse>> Register(RegisterRequest registerRequest, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = registerRequest.email.Trim().ToLower();
        if (await DbContext.Users.AnyAsync(u => u.Email == normalizedEmail, cancellationToken))
            return ApiResponse<RegisterResponse>.ErrorResponse("Email already exists");

        var user = new User
        {
            Email = normalizedEmail,
            Name = registerRequest.name,
            Role = registerRequest.role,
            PasswordHash = hasher.HashPassword(registerRequest.password)
        };

        DbContext.Users.Add(user);
        await DbContext.SaveChangesAsync(cancellationToken);

        var response = new RegisterResponse
        {
            Id = user.Id,
            name = user.Name,
            email = user.Email,
            token = tokenGenerator.GenerateToken(user)
        };

        return ApiResponse<RegisterResponse>.SuccessResponse(response);
    }

    public async Task<ApiResponse<LoginResponse>> Login(LoginRequest loginRequest, CancellationToken cancellationToken = default)
    {
        var user = await ValidateUserCredentials(loginRequest.email, loginRequest.password, cancellationToken);
        if (user == null)
            return ApiResponse<LoginResponse>.ErrorResponse("Invalid Credentials");

        var res = new LoginResponse
        {
            token = tokenGenerator.GenerateToken(user)
        };

        return ApiResponse<LoginResponse>.SuccessResponse(res);
    }

    public async Task<User?> ValidateUserCredentials(string email, string password, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim().ToLower();
        var user = await DbContext.Users.FirstOrDefaultAsync(u => u.Email == normalizedEmail, cancellationToken);
        if (user is null)
            return null;
        return hasher.VerifyPassword(password, user.PasswordHash) ? user : null;
    }
    


    public async Task<ApiResponse<User>> Me(CancellationToken cancellationToken = default)
    {
        User user = await DbContext.Users.FirstOrDefaultAsync(x => x.Id == CurrentUser.Id, cancellationToken);
        return ApiResponse<User>.SuccessResponse(user);
    }
}