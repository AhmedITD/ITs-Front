using Library.Domain.Entities.Common;
using Library.Domain.Enums;
using Library.Domain.Interfaces;

namespace Library.Domain.Entities;

public class User : BaseEntity, IAuditable, ISoftDeletable
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public UserRole Role { get; set; }
    public ICollection<Loan> Loans { get; set; } = new List<Loan>();

    // IAuditable
    public int? UpdatedBy { get; set; }

    // ISoftDeletable
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}