namespace RentARide.Application.DTOs.Requests.VehicleRequest;

public class BrowseVehiclesRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int? VehicleTypeId { get; set; }
}
