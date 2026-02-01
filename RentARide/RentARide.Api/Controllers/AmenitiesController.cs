using RentARide.Api.Common;
using RentARide.Application.Common;
using RentARide.Application.DTOs.Requests.AmenityRequest;
using RentARide.Application.DTOs.Responses.AmenityResponse;
using RentARide.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentARide.Application.DTOs.Responses.Common;

namespace RentARide.Api.Controllers;

[Route("amenities")]
public class AmenitiesController(IAmenityService amenityService) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<AmenityDto>>>> GetAmenities(CancellationToken cancellationToken)
    {
        var result = await amenityService.GetAmenities(cancellationToken);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<AmenityDto>>> CreateAmenity([FromBody] CreateAmenityRequest request, CancellationToken cancellationToken)
    {
        var result = await amenityService.CreateAmenity(request, cancellationToken);
        return result.Success ? Created($"/amenities/{result.Data!.Id}", result) : BadRequest(result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<AmenityDto>>> UpdateAmenity(int id, [FromBody] UpdateAmenityRequest request, CancellationToken cancellationToken)
    {
        var result = await amenityService.UpdateAmenity(id, request, cancellationToken);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteAmenity(int id, CancellationToken cancellationToken)
    {
        var result = await amenityService.DeleteAmenity(id, cancellationToken);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
