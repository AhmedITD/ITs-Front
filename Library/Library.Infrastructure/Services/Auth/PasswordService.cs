using Library.Application.Interfaces;
using Library.Application.Interfaces.Auth;
using Microsoft.Extensions.Logging;

namespace Library.Infrastructure.Services.Auth;

public class PasswordService : IPasswordService
{
    private readonly ILogger<PasswordService> _logger;

    public PasswordService(ILogger<PasswordService> logger)
    {
        _logger = logger;
    }

    public string HashPassword(string password)
    {
        try
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
            return hashedPassword;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error hashing password");
            throw;
        }
    }
    public bool VerifyPassword(string password, string hash)
    {
        try
        {
            var isValid = BCrypt.Net.BCrypt.Verify(password, hash);
            return isValid;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying password");
            throw;
        }
    }
}
