using Library.Application.Common;

namespace Library.Application.DTOs.responses.LoanResponse;
public class GetHistorcailByUserIdLoanResponse
{
    public required ApiResponse<HisData[]> Response { get; set; }
}

public class GetHistorcailByIdLoanResponse
{
    public int UserId { get; set; }
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime LoanDate { get; set; }
    public DateTime? ReturnDate { get; set; }
}

// Keep HisData for backward compatibility, but use HistoricalLoanResponse going forward
public class HisData
{
    public int UserId { get; set; }
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime LoanDate { get; set; }
    public DateTime? ReturnDate { get; set; }
}
