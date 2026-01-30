namespace RentARide.Application.DTOs.Responses.VehicleResponse;

public class VehicleTypeDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}
