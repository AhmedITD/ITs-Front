using RentARide.Domain.Enums;

namespace RentARide.Application.Interfaces.Auth;

public interface ICurrentUser
{
    int Id { get; }
    UserRole Role { get; }
}
