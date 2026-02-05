using System.ComponentModel.DataAnnotations;

namespace RentARide.Application.DTOs.requests.VehicleRequest;

public class CreateVehicleRequest
{
    public required string Model { get; set; }
    public int Year { get; set; }
    public required string LicensePlate { get; set; }
    public decimal DailyPrice { get; set; }
    public int VehicleTypeId { get; set; }
    public string? MaintenanceDescription { get; set; }
    public DateTime? LastMaintenanceDate { get; set; }
    public DateTime? NextMaintenanceDue { get; set; }
}
