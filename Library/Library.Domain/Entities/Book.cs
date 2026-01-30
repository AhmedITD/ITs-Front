using Library.Domain.Entities.Common;
using Library.Domain.Interfaces;

namespace Library.Domain.Entities;

public class Book : BaseEntity, IAuditable, ISoftDeletable
{
    public string Title { get; set; } = string.Empty;
    public ICollection<Loan> Loans { get; set; } = new List<Loan>();

    // IAuditable
    public int? UpdatedBy { get; set; }

    // ISoftDeletable
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}