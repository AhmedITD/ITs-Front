using Library.Api.Common;
using Library.Application.Common;
using Library.Application.Common.Pagination;
using Library.Application.DTOs;
using Library.Application.DTOs.requests;
using Library.Application.DTOs.requests.BookRequest;
using Library.Application.DTOs.requests.LoanRequest;
using Library.Application.DTOs.responses;
using Library.Application.DTOs.responses.LoanResponse;
using Library.Application.Interfaces;
using Library.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers;


public class LoanController(ILoanService LoanService) : BaseController
{
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<LoanResponse>> LoanBook(CreateLoanRequest request, CancellationToken cancellationToken)
    {
        ApiResponse<LoanResponse> response = await LoanService.LoanBook(request, cancellationToken);
        return response.Success ? Created($"/Loan/User/{response.Data?.UserId}/Book/{response.Data?.BookId}", response) : BadRequest(response);
    }

    [HttpGet]
    [Authorize("Admin")]

    public async Task<ActionResult<List<GetHistorcailByIdLoanResponse>>> GetHistorcailByUserIdLoan(
        [FromQuery] int userId,
        [FromQuery] int PageSize,
        [FromQuery] int PageNumber,
        CancellationToken cancellationToken)
    {
        ApiResponse<PaginatedList<GetHistorcailByIdLoanResponse>> response = await LoanService.GetHistorcailByIdLoanService(userId, PageSize, PageNumber, cancellationToken);
        return response.Success ? Ok(response) : BadRequest(response);
    }

    [HttpPut]
    [Authorize]
    public async Task<ActionResult<LoanResponse>> ReturnBook([FromBody] ReturnBookRequest request, CancellationToken cancellationToken)
    {
        ApiResponse<LoanResponse> response = await LoanService.ReturnBook(request, cancellationToken);
        return response.Success ? Ok(response) : BadRequest(response);
    }
}