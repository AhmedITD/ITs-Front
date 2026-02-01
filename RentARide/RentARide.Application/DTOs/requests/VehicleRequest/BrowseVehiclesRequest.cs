using RentARide.Application.DTOs.requests.Common;

namespace RentARide.Application.DTOs.Requests.VehicleRequest;

public class BrowseVehiclesRequest : PaginatedListRequest
{
    public int? VehicleTypeId { get; set; }
}
