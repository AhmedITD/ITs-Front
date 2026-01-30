using RentARide.Domain.Entities.Common;
using RentARide.Domain.Enums;
using RentARide.Domain.Interfaces;

namespace RentARide.Domain.Entities;

public class User : BaseEntity, IAuditable, ISoftDeletable
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public UserRole Role { get; set; }

    public ICollection<Rental> Rentals { get; set; } = new List<Rental>();

    public int? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}
