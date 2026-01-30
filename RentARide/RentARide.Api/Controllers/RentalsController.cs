using RentARide.Api.Common;
using RentARide.Application.Common;
using RentARide.Application.Common.Pagination;
using RentARide.Application.DTOs.Requests.RentalRequest;
using RentARide.Application.DTOs.Responses.RentalResponse;
using RentARide.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RentARide.Api.Controllers;

[Route("rentals")]
public class RentalsController(IRentalService rentalService) : BaseController
{
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ApiResponse<RentalDto>>> CreateRental([FromBody] CreateRentalRequest request, CancellationToken cancellationToken)
    {
        var result = await rentalService.CreateRental(request, cancellationToken);
        return result.Success ? Created($"/rentals/{result.Data!.Id}", result) : BadRequest(result);
    }

    [HttpGet("my-history")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<PaginatedList<RentalHistoryItemDto>>>> GetMyHistory(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await rentalService.GetMyHistory(pageNumber, pageSize, cancellationToken);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
