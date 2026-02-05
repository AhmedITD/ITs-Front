namespace RentARide.Application.DTOs.responses.VehicleResponse;

public class VehicleDto
{
    public int Id { get; set; }
    public required string Model { get; set; }
    public int Year { get; set; }
    public required string LicensePlate { get; set; }
    public decimal DailyPrice { get; set; }
    public required string Status { get; set; }
    public int VehicleTypeId { get; set; }
    public required string VehicleTypeName { get; set; }
}
