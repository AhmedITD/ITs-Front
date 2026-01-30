using RentARide.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RentARide.Infrastructure.Data.EntityConfigurations;

public class RentalAmenityConfiguration : IEntityTypeConfiguration<RentalAmenity>
{
    public void Configure(EntityTypeBuilder<RentalAmenity> builder)
    {
        builder.ToTable("RentalAmenities");
        builder.HasKey(x => new { x.RentalId, x.AmenityId });

        builder.HasOne(x => x.Rental)
            .WithMany(x => x.RentalAmenities)
            .HasForeignKey(x => x.RentalId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Amenity)
            .WithMany(x => x.RentalAmenities)
            .HasForeignKey(x => x.AmenityId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
