namespace RentARide.Application.DTOs.Requests.AmenityRequest;

public class CreateAmenityRequest
{
    public required string Name { get; set; }
    public decimal Price { get; set; }
}
