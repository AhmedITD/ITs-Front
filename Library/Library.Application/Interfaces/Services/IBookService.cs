using Library.Application.DTOs.responses;
using Library.Application.DTOs.requests;
using Library.Application.Common;
using Library.Application.Common.Pagination;
using Library.Application.DTOs;
using Library.Application.DTOs.requests.BookRequest;
using Library.Application.DTOs.responses.BookResponse;
using Library.Domain.Entities;

namespace Library.Application.Interfaces.Services;

public interface IBookService
{
    Task<ApiResponse<PaginatedList<Book>>> GetAllBooks(GetAllBooksRequest request, CancellationToken cancellationToken = default);
    Task<CreateBookResponse> CreateBook(CreateBookRequest createBookRequest, CancellationToken cancellationToken = default);
    Task<ApiResponse<UpdateBookResponse>> UpdateBook(UpdateBookRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<DeleteBookResponse>> DeleteBook(DeleteBookRequest request, CancellationToken cancellationToken = default);
}