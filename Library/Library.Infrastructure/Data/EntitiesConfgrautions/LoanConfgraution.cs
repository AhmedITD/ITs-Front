using Library.Application.Interfaces;
using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lib.Infrastructure.Data.EntitiesConfgrautions;

public class LoanConfgraution : IEntityTypeConfiguration<Loan>
{
    public void Configure(EntityTypeBuilder<Loan> builder)
    {
        builder.ToTable("Loans");

        
        builder.HasKey(x => new { x.UserId, x.BookId });
        
        builder.Property(x => x.UserId)
            .IsRequired();
        builder.Property(x => x.BookId)
            .IsRequired();
        
        builder.Property(x => x.LoanDate)
            .IsRequired()
            .HasDefaultValueSql("NOW()");
        
        builder.Property(x => x.ReturnDate)
            .IsRequired(false)
            .HasDefaultValue(null);
        
        builder
            .HasOne(x => x.User)
            .WithMany(x => x.Loans)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasOne(x => x.Book)
            .WithMany(x => x.Loans)
            .HasForeignKey(x => x.BookId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}