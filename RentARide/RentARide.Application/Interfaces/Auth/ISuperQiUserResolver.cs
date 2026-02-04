namespace RentARide.Application.Interfaces.Auth;

/// <summary>
/// Resolves SuperQi auth code to user information for creating/finding RentARide users.
/// </summary>
public interface ISuperQiUserResolver
{
    /// <summary>
    /// Validates the SuperQi auth code and returns user info. Returns null if validation fails.
    /// </summary>
    Task<SuperQiUserInfo?> ResolveAsync(string authCode, CancellationToken cancellationToken = default);
}

public class SuperQiUserInfo
{
    public required string UserId { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    
    /// <summary>Alipay+ access token (when resolved via Alipay service).</summary>
    public string? AccessToken { get; set; }
    
    /// <summary>Alipay+ customer ID (when resolved via Alipay service).</summary>
    public string? CustomerId { get; set; }
}
