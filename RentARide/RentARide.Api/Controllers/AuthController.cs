using RentARide.Api.Common;
using RentARide.Application.Common;
using RentARide.Application.DTOs.Requests.AuthRequest;
using RentARide.Application.DTOs.Responses.Common;
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

    [HttpPost("refresh")]
    public async Task<ActionResult<LoginResponse>> Refresh(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await authService.Refresh(request, cancellationToken);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>Exchange SuperQi auth code for JWT. Returns clear error message if code invalid/expired (resolver returns null).</summary>
    [HttpPost("auth-with-superQi")]
    public async Task<ActionResult<LoginResponse>> AuthWithSuperQi(AuthWithSuperQiRequest request, CancellationToken cancellationToken)
    {
        var result = await authService.AuthWithSuperQi(request, cancellationToken);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost("logout")]
    public async Task<ActionResult<ApiResponse<object>>> Logout(LogoutRequest request, CancellationToken cancellationToken)
    {
        var result = await authService.Logout(request, cancellationToken);
        return Ok(result);
    }
    
    [HttpPost("logoutAllDevices")]
    public async Task<ActionResult<ApiResponse<object>>> Logout(CancellationToken cancellationToken)
    {
        var result = await authService.LogoutAllDevices(cancellationToken);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("Me")]
    public async Task<ActionResult<ApiResponse<User>>> Me(CancellationToken cancellationToken)
    {
        var response = await authService.Me(cancellationToken);
        return response.Success ? Ok(response) : BadRequest(response);
    }
}
