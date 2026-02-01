using System.Linq.Expressions;
using RentARide.Application.Interfaces;
using RentARide.Domain.Entities;
using RentARide.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace RentARide.Infrastructure.Data;

public class RentARideDbContext : DbContext, IRentARideDbContext
{
    public RentARideDbContext(DbContextOptions<RentARideDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<VehicleType> VehicleTypes { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<VehicleMaintenance> VehicleMaintenances { get; set; }
    public DbSet<Rental> Rentals { get; set; }
    public DbSet<Amenity> Amenities { get; set; }
    public DbSet<RentalAmenity> RentalAmenities { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RentARideDbContext).Assembly);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType)) continue;
            modelBuilder.Entity(entityType.ClrType)
                .Property("IsDeleted")
                .HasDefaultValue(false);

            var parameter = Expression.Parameter(entityType.ClrType, "e");
            var property = Expression.Property(parameter, nameof(ISoftDeletable.IsDeleted));
            var falseConstant = Expression.Constant(false);
            var binaryExpression = Expression.Equal(property, falseConstant);
            var filter = Expression.Lambda(binaryExpression, parameter);

            modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);

            foreach (var index in entityType.GetIndexes())
            {
                if (string.IsNullOrEmpty(index.GetFilter()))
                    index.SetFilter("\"IsDeleted\" = false");
            }
        }
    }
}
