using RentARide.Application.DTOs.requests.Common;
using RentARide.Domain.Enums;

namespace RentARide.Application.DTOs.Requests.VehicleRequest;

public class BrowseVehiclesRequest : PaginatedListRequest
{
    public int? VehicleTypeId { get; set; }
    public VehicleStatus? Status { get; set; }
    public string? SearchTerm { get; set; }
}
