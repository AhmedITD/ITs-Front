using Library.Api.Common;
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers;


public class BookController(IBookService bookService) : BaseController
{
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CreateBookResponse>> CreateBook(CreateBookRequest request, CancellationToken cancellationToken)
    {
        CreateBookResponse createBook = await bookService.CreateBook(request, cancellationToken);
        return Created($"/Book/{createBook.Id}", createBook);
    }

    [HttpGet]
    public async Task<ActionResult<List<CreateBookResponse>>> GetAllBooks([FromQuery]GetAllBooksRequest request, CancellationToken cancellationToken)
    {
        ApiResponse<PaginatedList<Book>> books = await bookService.GetAllBooks(request, cancellationToken);
        return Ok(books);
    }
    [HttpPut]
    public async Task<ActionResult<UpdateBookResponse>> UpdateBook([FromBody]UpdateBookRequest request, CancellationToken cancellationToken)
    {
        ApiResponse<UpdateBookResponse> book = await bookService.UpdateBook(request, cancellationToken);
        return Ok(book);
    }

    [HttpDelete]
    public async Task<ActionResult<DeleteBookResponse>> DeleteBook([FromQuery]DeleteBookRequest request, CancellationToken cancellationToken)
    {
        ApiResponse<DeleteBookResponse> book = await bookService.DeleteBook(request, cancellationToken);
        return Ok(book);
    }
}