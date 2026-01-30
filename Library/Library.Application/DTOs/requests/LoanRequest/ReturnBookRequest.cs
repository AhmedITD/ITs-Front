using System.ComponentModel.DataAnnotations;

namespace Library.Application.DTOs.requests.BookRequest;
public class ReturnBookRequest
{
    [Required]
    public int BookId { get; set; }
}