using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.Interfaces;

public interface ILibraryDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Loan> Loans { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    int SaveChanges();
}