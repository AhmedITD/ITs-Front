namespace RentARide.Application.DTOs.Requests.VehicleRequest;

public class CreateVehicleTypeRequest
{
    public required string Name { get; set; }
    public string? Description { get; set; }
}
