using RentARide.Application.DTOs.Requests.QiCard;
using RentARide.Application.DTOs.Responses.QiCard;

namespace RentARide.Application.Interfaces;

public interface IQiCardService
{
    Task<QiCardResponse> InitiatePaymentAsync(QiCardPaymentRequest request, CancellationToken cancellationToken = default);
    Task<QiCardResponse> VerifyPaymentAsync(string paymentId, CancellationToken cancellationToken = default);
    Task<QiCardResponse> CancelPaymentAsync(string paymentId);
    Task<QiCardResponse> ProcessWebhookAsync(Dictionary<string, object> webhookData);
}
