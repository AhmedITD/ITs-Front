namespace RentARide.Application.DTOs.Responses.RentalResponse;

public class RentalDto
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalPrice { get; set; }
    public required string Status { get; set; }
    public int VehicleId { get; set; }
    public required string VehicleModel { get; set; }
    public List<string> AmenityNames { get; set; } = new();
}
