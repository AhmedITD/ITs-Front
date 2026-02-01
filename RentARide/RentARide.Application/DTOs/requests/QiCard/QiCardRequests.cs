namespace RentARide.Application.DTOs.Requests.QiCard;

public class QiCardPaymentRequest
{
    public string RequestId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "IQD";
    public string? FinishPaymentUrl { get; set; }
    public string? NotificationUrl { get; set; }
    public CustomerInfo? CustomerInfo { get; set; }
    public BrowserInfo? BrowserInfo { get; set; }
    public string? Description { get; set; }
}

/// <summary>Customer details aligned with User entity (no PasswordHash). Sent to QiCard.</summary>
public class CustomerInfo
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
}

/// <summary>Browser/client info for QiCard 3DS.</summary>
public class BrowserInfo
{
    public string? BrowserAcceptHeader { get; set; }
    public string? BrowserIp { get; set; }
    public bool BrowserJavaEnabled { get; set; } = false;
    public string? BrowserLanguage { get; set; }
    public string? BrowserUserAgent { get; set; }
}
