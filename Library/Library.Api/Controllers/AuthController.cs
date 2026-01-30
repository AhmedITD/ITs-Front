using Library.Api.Common;
using Library.Application.Common;
using Library.Application.DTOs;
using Library.Application.DTOs.responses;
using Library.Application.DTOs.requests;
using Library.Application.DTOs.requests.AuthRequest;
using Library.Application.DTOs.responses.AuthResponse;
using Library.Application.Interfaces;
using Library.Application.Interfaces.Auth;
using Library.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers;


public class AuthController(IAuthService authService) : BaseController
{
    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponse>> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        ApiResponse<RegisterResponse> result = await authService.Register(request, cancellationToken);
        return result.Success ? Created($"/User/{result.Data!.Id}", result) : BadRequest(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        ApiResponse<LoginResponse> result = await authService.Login(request, cancellationToken);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [Authorize("Admin")]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<User>>>> GetAllUsers(CancellationToken cancellationToken)
    {
        ApiResponse<List<User>> users = await authService.GetAllUsers(cancellationToken);
        return users.Success ? Ok(users) : BadRequest(users);
    }
    [Authorize]
    [HttpGet("Me")]
    public async Task<ActionResult<ApiResponse<User>>> Me(CancellationToken cancellationToken)
    {
        ApiResponse<User> response = await authService.Me(cancellationToken);
        return response.Success ? Ok(response) : BadRequest(response);
    }
}