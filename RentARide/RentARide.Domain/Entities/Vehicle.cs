using RentARide.Domain.Entities.Common;
using RentARide.Domain.Enums;
using RentARide.Domain.Interfaces;

namespace RentARide.Domain.Entities;

public class Vehicle : BaseEntity, IAuditable, ISoftDeletable
{
    public required string Model { get; set; }
    public int Year { get; set; }
    public required string LicensePlate { get; set; }
    public decimal DailyPrice { get; set; }
    public VehicleStatus Status { get; set; }
    public int VehicleTypeId { get; set; }

    public VehicleType VehicleType { get; set; } = null!;
    public VehicleMaintenance? VehicleMaintenance { get; set; }
    public ICollection<Rental> Rentals { get; set; } = new List<Rental>();

    public int? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}
