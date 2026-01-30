namespace Library.Application.DTOs.responses.LoanResponse;
public class LoanResponse
{
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public int BookId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public DateTime LoanDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
