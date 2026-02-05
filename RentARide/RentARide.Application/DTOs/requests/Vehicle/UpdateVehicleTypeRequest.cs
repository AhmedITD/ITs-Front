namespace RentARide.Application.DTOs.requests.VehicleRequest;

public class UpdateVehicleTypeRequest
{
    public required string Name { get; set; }
    public string? Description { get; set; }
}
