using Library.Application.Common;
using Library.Application.Common.Pagination;
using Library.Application.DTOs;
using Library.Application.DTOs.requests;
using Library.Application.DTOs.requests.BookRequest;
using Library.Application.DTOs.requests.LoanRequest;
using Library.Application.DTOs.responses;
using Library.Application.DTOs.responses.LoanResponse;
using Library.Application.Interfaces;
using Library.Application.Interfaces.Auth;
using Library.Application.Interfaces.Services;
using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.Services;

public class LoanService(ILibraryDbContext DbContext, ICurrentUser currentUser) : ILoanService
{
    public async Task<ApiResponse<LoanResponse>> LoanBook(CreateLoanRequest createLoanRequest, CancellationToken cancellationToken = default)
    {
        Book? book = await DbContext.Books.FirstOrDefaultAsync(x => x.Id == createLoanRequest.BookId, cancellationToken);
        if (book == null) return ApiResponse<LoanResponse>.ErrorResponse("Book not found");

        Loan? activeLoan = await DbContext.Loans.FirstOrDefaultAsync(x => x.BookId == createLoanRequest.BookId && x.ReturnDate == null, cancellationToken);
        if (activeLoan != null) return ApiResponse<LoanResponse>.ErrorResponse("Book is already loaned");

        Loan? existing = await DbContext.Loans
            .Include(l => l.Book)
            .Include(l => l.User)
            .FirstOrDefaultAsync(x => x.UserId == currentUser.Id && x.BookId == createLoanRequest.BookId, cancellationToken);

        if (existing != null)
        {
            existing.ReturnDate = null;
            existing.LoanDate = DateTime.UtcNow;
            await DbContext.SaveChangesAsync(cancellationToken);

            var loanResponse = new LoanResponse
            {
                UserId = existing.UserId,
                UserName = existing.User.Name,
                BookId = existing.BookId,
                BookTitle = existing.Book.Title,
                LoanDate = existing.LoanDate,
                ReturnDate = existing.ReturnDate
            };

            return ApiResponse<LoanResponse>.SuccessResponse(loanResponse);
        }

        var newLoan = new Loan
        {
            BookId = createLoanRequest.BookId,
            UserId = currentUser.Id,
            LoanDate = DateTime.UtcNow,
            ReturnDate = null
        };
        DbContext.Loans.Add(newLoan);
        await DbContext.SaveChangesAsync(cancellationToken);

        var loanWithNav = await DbContext.Loans
            .Include(l => l.Book)
            .Include(l => l.User)
            .FirstOrDefaultAsync(x => x.UserId == newLoan.UserId && x.BookId == newLoan.BookId, cancellationToken);

        var _loanResponse = new LoanResponse
        {
            UserId = loanWithNav!.UserId,
            UserName = loanWithNav.User.Name,
            BookId = loanWithNav.BookId,
            BookTitle = loanWithNav.Book.Title,
            LoanDate = loanWithNav.LoanDate,
            ReturnDate = loanWithNav.ReturnDate
        };

        return ApiResponse<LoanResponse>.SuccessResponse(_loanResponse);
    }

    public async Task<ApiResponse<LoanResponse>> ReturnBook(ReturnBookRequest returnBookRequest, CancellationToken cancellationToken = default)
    {
        Loan? loan = await DbContext.Loans
            .Include(l => l.Book)
            .Include(l => l.User)
            .FirstOrDefaultAsync(x => x.BookId == returnBookRequest.BookId && x.UserId == currentUser.Id && x.ReturnDate == null, cancellationToken);
        if (loan == null) return ApiResponse<LoanResponse>.ErrorResponse("No active loan found");
        loan.ReturnDate = DateTime.UtcNow;
        await DbContext.SaveChangesAsync(cancellationToken);

        var loanResponse = new LoanResponse
        {
            UserId = loan.UserId,
            UserName = loan.User.Name,
            BookId = loan.BookId,
            BookTitle = loan.Book.Title,
            LoanDate = loan.LoanDate,
            ReturnDate = loan.ReturnDate
        };

        return ApiResponse<LoanResponse>.SuccessResponse(loanResponse);
    }

    public async Task<ApiResponse<PaginatedList<GetHistorcailByIdLoanResponse>>> GetHistorcailByIdLoanService(int userId, int PageSize, int PageNumber, CancellationToken cancellationToken = default)
    {
        var hisLoans = DbContext.Loans
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .Include(b => b.Book)
            .Select(x => new GetHistorcailByIdLoanResponse
            {
                UserId = x.UserId,
                BookId = x.BookId,
                Title = x.Book.Title,
                LoanDate = x.LoanDate,
                ReturnDate = x.ReturnDate
            }).OrderBy(Date => Date.LoanDate);

        var pagedHisLoans = await PaginatedList<GetHistorcailByIdLoanResponse>.CreateAsync(hisLoans, PageNumber, PageSize, cancellationToken);

        return ApiResponse<PaginatedList<GetHistorcailByIdLoanResponse>>.SuccessResponse(pagedHisLoans);
    }
}
