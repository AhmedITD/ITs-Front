using RentARide.Api.Common;
using RentARide.Application.DTOs.Requests.RentalRequest;
using RentARide.Application.DTOs.Responses.RentalResponse;
using RentARide.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentARide.Application.DTOs.requests.Invoice;
using RentARide.Application.DTOs.Responses.Common;
using RentARide.Application.DTOs.responses.Invoice;
using RentARide.Application.Services;

namespace RentARide.Api.Controllers;

[Route("rentals")]
public class RentalsController(IRentalService rentalService) : BaseController
{
    [HttpGet("my-history")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<PaginatedListResponse<RentalHistoryItemDto>>>> GetMyHistory(
        [FromQuery] GetMyRentalsRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await rentalService.GetMyHistory(request, cancellationToken);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
