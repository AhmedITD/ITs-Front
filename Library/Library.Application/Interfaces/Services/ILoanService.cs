using Library.Application.Common;
using Library.Application.Common.Pagination;
using Library.Application.DTOs;
using Library.Application.DTOs.responses;
using Library.Application.DTOs.requests;
using Library.Application.DTOs.requests.BookRequest;
using Library.Application.DTOs.requests.LoanRequest;
using Library.Application.DTOs.responses.LoanResponse;

namespace Library.Application.Interfaces.Services;

public interface ILoanService
{
    Task<ApiResponse<LoanResponse>> LoanBook(CreateLoanRequest createLoanRequest, CancellationToken cancellationToken = default);
    Task<ApiResponse<LoanResponse>> ReturnBook(ReturnBookRequest bookId, CancellationToken cancellationToken = default);
    Task<ApiResponse<PaginatedList<GetHistorcailByIdLoanResponse>>> GetHistorcailByIdLoanService(int id, int PageSize, int PageNumber, CancellationToken cancellationToken = default);
}
