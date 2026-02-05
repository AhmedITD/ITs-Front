namespace RentARide.Application.DTOs.requests.RentalRequest;

public class CreateRentalRequest             
{
    public int VehicleId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<int> AmenityIds { get; set; } = new();
}
