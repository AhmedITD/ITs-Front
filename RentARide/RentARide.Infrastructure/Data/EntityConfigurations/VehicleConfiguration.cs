using RentARide.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RentARide.Infrastructure.Data.EntityConfigurations;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("Vehicles");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Model).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Year).IsRequired();
        builder.Property(x => x.LicensePlate).IsRequired().HasMaxLength(20);
        builder.Property(x => x.DailyPrice).HasPrecision(18, 2);
        builder.Property(x => x.Status).IsRequired();
        builder.Property(x => x.VehicleTypeId).IsRequired();

        builder.HasOne(x => x.VehicleType)
            .WithMany(x => x.Vehicles)
            .HasForeignKey(x => x.VehicleTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.VehicleMaintenance)
            .WithOne(x => x.Vehicle)
            .HasForeignKey<VehicleMaintenance>(x => x.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
