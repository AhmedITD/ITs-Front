namespace RentARide.Domain.Entities;

public class VehicleMaintenance
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public string? Description { get; set; }
    public DateTime? LastMaintenanceDate { get; set; }
    public DateTime? NextMaintenanceDue { get; set; }

    public Vehicle Vehicle { get; set; } = null!;
}
