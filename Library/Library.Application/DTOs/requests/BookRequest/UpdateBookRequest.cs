namespace Library.Application.DTOs.requests.BookRequest;
public class UpdateBookRequest
{
    public required int Id { get; set; }
    public required string Title { get; set; }
}