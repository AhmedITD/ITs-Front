using RentARide.Api.Common;
using RentARide.Application.Common;
using RentARide.Application.DTOs.Requests.AuthRequest;
using RentARide.Application.DTOs.Responses.AuthResponse;
using RentARide.Application.Interfaces.Auth;
using RentARide.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RentARide.Api.Controllers;

[Route("auth")]
public class AuthController(IAuthService authService) : BaseController
{
    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponse>> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await authService.Register(request, cancellationToken);
        return result.Success ? Created($"/User/{result.Data!.Id}", result) : BadRequest(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await authService.Login(request, cancellationToken);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [Authorize]
    [HttpGet("Me")]
    public async Task<ActionResult<ApiResponse<User>>> Me(CancellationToken cancellationToken)
    {
        var response = await authService.Me(cancellationToken);
        return response.Success ? Ok(response) : BadRequest(response);
    }
}
