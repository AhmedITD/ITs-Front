namespace RentARide.Application.DTOs.Requests.VehicleRequest;

public class UpdateVehicleTypeRequest
{
    public required string Name { get; set; }
    public string? Description { get; set; }
}
