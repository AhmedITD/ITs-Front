using RentARide.Application.DTOs.Requests.QiCard;
using RentARide.Application.DTOs.Responses.QiCard;

namespace RentARide.Application.Interfaces;

public interface IQiCardService
{
    Task<QiCardResponse> InitiatePaymentAsync(QiCardPaymentRequest request, CancellationToken cancellationToken = default);
    Task<QiCardResponse> VerifyPaymentAsync(string paymentId, CancellationToken cancellationToken = default);
    Task<QiCardResponse> CancelPaymentAsync(string paymentId);
    /// <summary>Verifies webhook message using raw body and signature from header (QiCard message verification).</summary>
    bool VerifyWebhookSignature(string rawBody, string? signatureFromHeader);
    Task<QiCardResponse> ProcessWebhookAsync(Dictionary<string, object> webhookData);
}
