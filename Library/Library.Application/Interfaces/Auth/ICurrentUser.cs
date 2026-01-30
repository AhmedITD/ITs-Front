using Library.Domain.Enums;

namespace Library.Application.Interfaces.Auth;

public interface ICurrentUser
{
    int Id { get; }
    UserRole Role { get; }
}