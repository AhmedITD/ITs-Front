using RentARide.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace RentARide.Application.Interfaces;

public interface IRentARideDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<VehicleType> VehicleTypes { get; set; }
    DbSet<Vehicle> Vehicles { get; set; }
    DbSet<VehicleMaintenance> VehicleMaintenances { get; set; }
    DbSet<Rental> Rentals { get; set; }
    DbSet<Amenity> Amenities { get; set; }
    DbSet<RentalAmenity> RentalAmenities { get; set; }
    DbSet<AuditLog> AuditLogs { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    int SaveChanges();
}
