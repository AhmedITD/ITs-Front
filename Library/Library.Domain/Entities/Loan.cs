using System.Text.Json.Serialization;
using Library.Domain.Entities.Common;
using Library.Domain.Interfaces;

namespace Library.Domain.Entities;

public class Loan : IAuditable
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public int BookId { get; set; }
    public Book Book { get; set; } = null!;
    public DateTime LoanDate { get; set; }
    public DateTime? ReturnDate { get; set; }

    // IAuditable
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
}