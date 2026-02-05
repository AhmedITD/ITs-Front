namespace RentARide.Application.DTOs.requests.VehicleRequest;

public class CreateVehicleTypeRequest
{
    public required string Name { get; set; }
    public string? Description { get; set; }
}
