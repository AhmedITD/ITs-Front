namespace RentARide.Application.DTOs.requests.AmenityRequest;

public class CreateAmenityRequest
{
    public required string Name { get; set; }
    public decimal Price { get; set; }
}
