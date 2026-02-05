using RentARide.Application.DTOs.requests.Common;
using RentARide.Domain.Enums;

namespace RentARide.Application.DTOs.requests.RentalRequest;

public class GetMyRentalsRequest : PaginatedListRequest
{
    public RentalStatus? Status { get; set; }
    public DateTime? StartDateFrom { get; set; }
    public DateTime? StartDateTo { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string? SearchTerm { get; set; }
}
