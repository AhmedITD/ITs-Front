using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentARide.Domain.Entities;

namespace RentARide.Infrastructure.Data.EntityConfigurations;

public class UserExternalAuthConfiguration : IEntityTypeConfiguration<UserExternalAuth>
{
    public void Configure(EntityTypeBuilder<UserExternalAuth> builder)
    {
        builder.ToTable("UserExternalAuths");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.Provider).IsRequired().HasMaxLength(64);
        builder.Property(x => x.ExternalUserId).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => new { x.Provider, x.ExternalUserId }).IsUnique();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
