using RentARide.Domain.Entities;

namespace RentARide.Application.Interfaces.Auth;

public interface IRefreshTokenService
{
    /// <summary>
    /// Creates a new refresh token for the user. Returns the plain token (to send to client) and its expiry.
    /// </summary>
    Task<(string RefreshToken, DateTime ExpiresAt)> CreateAsync(
        int userId,
        string? deviceClientId = null,
        string? ipAddress = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates the refresh token. Returns the user if valid and not revoked/expired; otherwise null.
    /// </summary>
    Task<User?> ValidateAsync(string refreshToken, CancellationToken cancellationToken = default);

    /// <summary>
    /// Revokes the refresh token (e.g. on logout).
    /// </summary>
    Task RevokeAsync(string refreshToken, CancellationToken cancellationToken = default);

    /// <summary>
    /// Revokes all refresh tokens for the user (e.g. on password change).
    /// </summary>
    Task RevokeAllForUserAsync(int userId, CancellationToken cancellationToken = default);
}
