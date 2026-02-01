using RentARide.Api.Common;
using RentARide.Application.DTOs.Requests.VehicleRequest;
using RentARide.Application.DTOs.Responses.VehicleResponse;
using RentARide.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentARide.Application.DTOs.Responses.Common;

namespace RentARide.Api.Controllers;

[Route("vehicles")]
public class VehiclesController(IVehicleService vehicleService) : BaseController
{
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<VehicleDto>>> CreateVehicle([FromBody] CreateVehicleRequest request, CancellationToken cancellationToken)
    {
        var result = await vehicleService.CreateVehicle(request, cancellationToken);
        return result.Success ? Created($"/vehicles/{result.Data!.Id}", result) : BadRequest(result);
    }

    [HttpPut("{id:int}/price")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<VehicleDto>>> UpdatePrice(int id, [FromBody] UpdateVehiclePriceRequest request, CancellationToken cancellationToken)
    {
        var result = await vehicleService.UpdatePrice(id, request, cancellationToken);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<object>>> SoftDeleteVehicle(int id, CancellationToken cancellationToken)
    {
        var result = await vehicleService.SoftDeleteVehicle(id, cancellationToken);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("types")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<VehicleTypeDto>>>> GetVehicleTypes(CancellationToken cancellationToken)
    {
        var result = await vehicleService.GetVehicleTypes(cancellationToken);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost("types")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<VehicleTypeDto>>> CreateVehicleType([FromBody] CreateVehicleTypeRequest request, CancellationToken cancellationToken)
    {
        var result = await vehicleService.CreateVehicleType(request, cancellationToken);
        return result.Success ? Created($"/vehicles/types/{result.Data!.Id}", result) : BadRequest(result);
    }

    [HttpPut("types/{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<VehicleTypeDto>>> UpdateVehicleType(int id, [FromBody] UpdateVehicleTypeRequest request, CancellationToken cancellationToken)
    {
        var result = await vehicleService.UpdateVehicleType(id, request, cancellationToken);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("types/{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteVehicleType(int id, CancellationToken cancellationToken)
    {
        var result = await vehicleService.DeleteVehicleType(id, cancellationToken);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PaginatedListResponse<VehicleDto>>>> BrowseVehicles([FromQuery] BrowseVehiclesRequest request, CancellationToken cancellationToken)
    {
        var result = await vehicleService.BrowseVehicles(request, cancellationToken);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
