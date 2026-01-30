using Library.Application.Common;
using Library.Application.Common.Pagination;
using Library.Application.DTOs;
using Library.Application.DTOs.requests;
using Library.Application.DTOs.requests.BookRequest;
using Library.Application.DTOs.responses;
using Library.Application.DTOs.responses.BookResponse;
using Library.Application.Interfaces;
using Library.Application.Interfaces.Services;
using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.Services;

public class BookService(ILibraryDbContext DbContext, IMemoryCacheService cache) : IBookService
{
    private const string BooksAllKey = "books_all";
    private static readonly TimeSpan BooksCacheDuration = TimeSpan.FromSeconds(30);

    public async Task<ApiResponse<PaginatedList<Book>>> GetAllBooks(GetAllBooksRequest request, CancellationToken cancellationToken = default)
    {
        var cachedList = cache.Get<List<Book>>(BooksAllKey);
        if (cachedList == null)
        {
            var books = DbContext.Books.OrderBy(x => x.Title);
            cachedList = await books.ToListAsync(cancellationToken);
            cache.Set(BooksAllKey, cachedList, BooksCacheDuration);
        }

        var totalCount = cachedList.Count;
        var pageItems = cachedList
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();
        var paged = new PaginatedList<Book>(pageItems, totalCount, request.PageNumber, request.PageSize);
        return ApiResponse<PaginatedList<Book>>.SuccessResponse(paged);
    }

    public async Task<CreateBookResponse> CreateBook(CreateBookRequest createBookRequest, CancellationToken cancellationToken = default)
    {
        var book = new Book
        {
            Title = createBookRequest.Title
        };
        DbContext.Books.Add(book);
        await DbContext.SaveChangesAsync(cancellationToken);
        cache.Remove(BooksAllKey);

        return new CreateBookResponse
        {
            Id = book.Id,
            Title = book.Title,
            CreatedAt = book.CreatedAt,
            UpdatedAt = book.UpdatedAt
        };
    }

    public async Task<ApiResponse<UpdateBookResponse>> UpdateBook(UpdateBookRequest updateBookRequest, CancellationToken cancellationToken = default)
    {
        Book? book = await DbContext.Books.FirstOrDefaultAsync(x => x.Id == updateBookRequest.Id, cancellationToken);
        if (book == null) return ApiResponse<UpdateBookResponse>.ErrorResponse("Book not found");
        
        book.Title = updateBookRequest.Title;
        book.UpdatedAt = DateTime.UtcNow;
        await DbContext.SaveChangesAsync(cancellationToken);
        
        return ApiResponse<UpdateBookResponse>.SuccessResponse(new UpdateBookResponse
        {
            Id = book.Id,
            Title = book.Title,
            CreatedAt = book.CreatedAt,
            UpdatedAt = book.UpdatedAt
        });
    }
    
    public async Task<ApiResponse<DeleteBookResponse>> DeleteBook(DeleteBookRequest deleteBookRequest, CancellationToken cancellationToken = default)
    {
        Book? book = await DbContext.Books.FirstOrDefaultAsync(x => x.Id == deleteBookRequest.Id, cancellationToken);
        if (book == null) return ApiResponse<DeleteBookResponse>.ErrorResponse("Book not found");
        //remove the book from the database
        DbContext.Books.Remove(book);
        await DbContext.SaveChangesAsync(cancellationToken);
        
        return ApiResponse<DeleteBookResponse>.SuccessResponse(new DeleteBookResponse
        {
            Id = book.Id,
            Title = book.Title,
            CreatedAt = book.CreatedAt,
            UpdatedAt = book.UpdatedAt
        });
    }
}