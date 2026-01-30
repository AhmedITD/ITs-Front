using System.Xml;
using Library.Application.Interfaces;
using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lib.Infrastructure.Data.EntitiesConfgrautions;

public class UserConfgraution : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(50);
        builder.HasIndex(x => x.Email)
            .IsUnique();
    }
}