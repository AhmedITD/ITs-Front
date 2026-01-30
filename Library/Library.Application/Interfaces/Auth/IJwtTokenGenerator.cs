namespace Library.Application.Interfaces.Auth;

using Library.Domain.Entities;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}