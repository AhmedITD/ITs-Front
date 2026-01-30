using RentARide.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RentARide.Infrastructure.Data.EntityConfigurations;

public class VehicleMaintenanceConfiguration : IEntityTypeConfiguration<VehicleMaintenance>
{
    public void Configure(EntityTypeBuilder<VehicleMaintenance> builder)
    {
        builder.ToTable("VehicleMaintenances");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.VehicleId).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(1000);
    }
}
