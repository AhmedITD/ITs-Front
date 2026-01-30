using RentARide.Domain.Entities;

namespace RentARide.Application.Interfaces.Auth;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
