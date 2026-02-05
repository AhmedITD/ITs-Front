using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RentARide.Application.DTOs.requests.SuperQi;
using RentARide.Application.DTOs.responses.Common;
using RentARide.Application.DTOs.responses.SuperQi;
using RentARide.Application.Interfaces;
using RentARide.Application.Interfaces.Services;
using RentARide.Domain.Enums;

namespace RentARide.Application.Services;

public class SuperQiMiniAppService : ISuperQiMiniAppService
{
    private readonly ISuperQiAlipayService _alipayService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<SuperQiMiniAppService> _logger;
    private readonly IRentARideDbContext _dbContext;

    // Product codes from SuperQiConstants
    private const string ONLINE_PURCHASE = "51051000101000000011";
    private const string AGREEMENT_PAYMENT = "51051000101000100031";
    private const string SUPERQI_WEBHOOK_PATH = "/api/payments/superqi-webhook";

    public SuperQiMiniAppService(
        ISuperQiAlipayService alipayService,
        IConfiguration configuration,
        ILogger<SuperQiMiniAppService> logger,
        IRentARideDbContext dbContext)
    {
        _alipayService = alipayService;
        _configuration = configuration;
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<ApiResponse<SuperQiPaymentResponse>> CreatePaymentAsync(string userId, CancellationToken ct = default)
    {
        _logger.LogInformation("[SuperQiMiniApp] Creating payment for user: {UserId}", userId);

        try
        {
            var paymentRequestId = $"PAY-{Guid.NewGuid()}-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";
            var expiryTime = DateTime.UtcNow.AddMinutes(30).ToString("yyyy-MM-ddTHH:mm:sszzz").Replace("+00:00", "+00:00");
            var baseUrl = _configuration["APP_URL"];

            var paymentRequest = new AlipayPaymentRequest
            {
                ProductCode = ONLINE_PURCHASE,
                PaymentRequestId = paymentRequestId,
                PaymentAmount = new AlipayAmount
                {
                    Currency = "IQD",
                    Value = "1000" // Test payment: 1 IQD (value in fils)
                },
                Order = new AlipayOrder
                {
                    OrderDescription = "Test Order - Online Purchase",
                    Buyer = new AlipayBuyer
                    {
                        ReferenceBuyerId = userId
                    }
                },
                PaymentExpiryTime = expiryTime,
                PaymentRedirectUrl = baseUrl + "/payment-redirect",
                PaymentNotifyUrl = baseUrl + SUPERQI_WEBHOOK_PATH
            };

            _logger.LogInformation("[SuperQiMiniApp] Payment request: {RequestId}", paymentRequestId);

            var paymentResponse = await _alipayService.PayAsync(paymentRequest, ct);

            var response = new SuperQiPaymentResponse
            {
                Amount = 1
            };

            if (paymentResponse.RedirectActionForm?.RedirectUrl != null)
            {
                response.Success = true;
                response.PaymentUrl = paymentResponse.RedirectActionForm.RedirectUrl;
                response.PaymentId = paymentResponse.PaymentId;
                _logger.LogInformation("[SuperQiMiniApp] Payment URL received: {PaymentId}", paymentResponse.PaymentId);
            }
            else if (paymentResponse.Result.ResultStatus == "S")
            {
                response.Success = true;
                response.PaymentId = paymentResponse.PaymentId;
                _logger.LogInformation("[SuperQiMiniApp] Payment completed immediately: {PaymentId}", paymentResponse.PaymentId);
            }
            else
            {
                response.Success = false;
                response.Error = paymentResponse.Result.ResultMessage ?? "No redirect URL received from payment API";
                _logger.LogWarning("[SuperQiMiniApp] Payment failed: {Message}", response.Error);
            }

            return ApiResponse<SuperQiPaymentResponse>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[SuperQiMiniApp] Payment creation failed");
            return ApiResponse<SuperQiPaymentResponse>.ErrorResponse(ex.Message);
        }
    }

    public async Task<ApiResponse<SuperQiRefundResponse>> RefundPaymentAsync(string paymentId, decimal amount, CancellationToken ct = default)
    {
        _logger.LogInformation("[SuperQiMiniApp] Processing refund: PaymentId={PaymentId}, Amount={Amount}", paymentId, amount);

        if (string.IsNullOrEmpty(paymentId))
            return ApiResponse<SuperQiRefundResponse>.ErrorResponse("Payment ID is required");

        if (amount <= 0)
            return ApiResponse<SuperQiRefundResponse>.ErrorResponse("Refund amount must be greater than 0");

        try
        {
            var refundRequestId = $"REFUND-{Guid.NewGuid()}-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";
            var amountInFils = (int)Math.Floor(amount * 1000);

            var refundRequest = new AlipayRefundRequest
            {
                RefundRequestId = refundRequestId,
                PaymentId = paymentId,
                RefundAmount = new AlipayAmount
                {
                    Currency = "IQD",
                    Value = amountInFils.ToString()
                },
                RefundReason = "Customer requested refund from mini app"
            };

            var refundResponse = await _alipayService.RefundAsync(refundRequest, ct);

            // Handle unknown status with polling
            if (refundResponse.Result.ResultStatus == "U")
            {
                _logger.LogInformation("[SuperQiMiniApp] Refund status unknown, polling...");
                var polledResponse = await PollRefundStatusAsync(refundRequestId, ct);
                if (polledResponse != null)
                    refundResponse = polledResponse;
            }

            var response = BuildRefundResponse(refundResponse);
            return ApiResponse<SuperQiRefundResponse>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[SuperQiMiniApp] Refund failed");
            return ApiResponse<SuperQiRefundResponse>.ErrorResponse(ex.Message);
        }
    }

    public async Task<ApiResponse<SuperQiNotificationResponse>> SendInboxAsync(
        string accessToken, string title, string content, string? url, CancellationToken ct = default)
    {
        _logger.LogInformation("[SuperQiMiniApp] Sending inbox notification: Title={Title}", title);

        try
        {
            var requestId = $"NOTIF-{Guid.NewGuid()}-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";
            var targetUrl = url ?? "mini://platformapi/startapp?_ariver_appid=888888";

            var request = new AlipayNotificationRequest
            {
                AccessToken = accessToken,
                RequestId = requestId,
                TemplateCode = "MINI_APP_COMMON_INBOX",
                Templates = new List<AlipayNotificationTemplate>
                {
                    new()
                    {
                        TemplateParameters = new Dictionary<string, string>
                        {
                            ["Title"] = title,
                            ["Content"] = content,
                            ["Url"] = targetUrl
                        }
                    }
                }
            };

            var notifResponse = await _alipayService.SendInboxAsync(request, ct);
            var response = BuildNotificationResponse(notifResponse);

            return ApiResponse<SuperQiNotificationResponse>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[SuperQiMiniApp] Send inbox failed");
            return ApiResponse<SuperQiNotificationResponse>.ErrorResponse(ex.Message);
        }
    }

    public async Task<ApiResponse<SuperQiNotificationResponse>> SendPushAsync(
        string accessToken, string title, string content, string? url, CancellationToken ct = default)
    {
        _logger.LogInformation("[SuperQiMiniApp] Sending push notification: Title={Title}", title);

        try
        {
            var requestId = $"NOTIF-{Guid.NewGuid()}-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";
            var targetUrl = url ?? "mini://platformapi/startapp?_ariver_appid=888888";

            var request = new AlipayNotificationRequest
            {
                AccessToken = accessToken,
                RequestId = requestId,
                TemplateCode = "MINI_APP_COMMON_PUSH",
                Templates = new List<AlipayNotificationTemplate>
                {
                    new()
                    {
                        TemplateParameters = new Dictionary<string, string>
                        {
                            ["Title"] = title,
                            ["Content"] = content,
                            ["Url"] = targetUrl
                        }
                    }
                }
            };

            var pushResponse = await _alipayService.SendPushAsync(request, ct);
            var response = BuildNotificationResponse(pushResponse);

            return ApiResponse<SuperQiNotificationResponse>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[SuperQiMiniApp] Send push failed");
            return ApiResponse<SuperQiNotificationResponse>.ErrorResponse(ex.Message);
        }
    }

    public async Task<ApiResponse<SuperQiPrepareAuthResponse>> PrepareAgreementAsync(string contractDescription, CancellationToken ct = default)
    {
        _logger.LogInformation("[SuperQiMiniApp] Preparing agreement: {Description}", contractDescription);

        try
        {
            var prepareResponse = await _alipayService.PrepareAuthorizationAsync(contractDescription, ct);

            var response = new SuperQiPrepareAuthResponse
            {
                Success = prepareResponse.Result.ResultStatus == "S",
                AuthUrl = prepareResponse.AuthUrl,
                ResultStatus = prepareResponse.Result.ResultStatus,
                ResultCode = prepareResponse.Result.ResultCode,
                ResultMessage = prepareResponse.Result.ResultMessage
            };

            if (!response.Success)
            {
                _logger.LogWarning("[SuperQiMiniApp] Agreement preparation failed: {Message}", response.ResultMessage);
                return ApiResponse<SuperQiPrepareAuthResponse>.ErrorResponse(response.ResultMessage ?? "Failed to prepare agreement");
            }

            return ApiResponse<SuperQiPrepareAuthResponse>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[SuperQiMiniApp] Prepare agreement failed");
            return ApiResponse<SuperQiPrepareAuthResponse>.ErrorResponse(ex.Message);
        }
    }

    public async Task<ApiResponse<SuperQiApplyTokenResponse>> ApplyAgreementTokenAsync(string authCode, CancellationToken ct = default)
    {
        _logger.LogInformation("[SuperQiMiniApp] Applying token for agreement");

        try
        {
            var tokenResponse = await _alipayService.ApplyTokenAsync(authCode, ct);

            var response = new SuperQiApplyTokenResponse
            {
                ResultStatus = tokenResponse.Result.ResultStatus,
                ResultCode = tokenResponse.Result.ResultCode,
                ResultMessage = tokenResponse.Result.ResultMessage
            };

            if (tokenResponse.Result.ResultStatus == "S" && tokenResponse.Result.ResultCode == "SUCCESS")
            {
                response.Success = true;
                response.AccessToken = tokenResponse.AccessToken;
                response.CustomerId = tokenResponse.CustomerId;
                response.AccessTokenExpiryTime = tokenResponse.AccessTokenExpiryTime;
            }
            else
            {
                response.Success = false;
                _logger.LogWarning("[SuperQiMiniApp] Token application failed: {Message}", tokenResponse.Result.ResultMessage);
                return ApiResponse<SuperQiApplyTokenResponse>.ErrorResponse(tokenResponse.Result.ResultMessage ?? "Token application failed");
            }

            return ApiResponse<SuperQiApplyTokenResponse>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[SuperQiMiniApp] Apply token failed");
            return ApiResponse<SuperQiApplyTokenResponse>.ErrorResponse(ex.Message);
        }
    }

    public async Task<ApiResponse<SuperQiPaymentResponse>> ExecuteAgreementPaymentAsync(
        SuperQiAgreementPayRequest request, CancellationToken ct = default)
    {
        _logger.LogInformation("[SuperQiMiniApp] Executing agreement payment: CustomerId={CustomerId}, Amount={Amount}, InvoiceId={InvoiceId}", request.CustomerId, request.Amount, request.InvoiceId);

        if (request.Amount <= 0)
            return ApiResponse<SuperQiPaymentResponse>.ErrorResponse("Payment amount must be greater than 0");

        Domain.Entities.Invoice? invoice = null;
        if (request.InvoiceId.HasValue)
        {
            invoice = await _dbContext.Invoices
                .FirstOrDefaultAsync(i => i.Id == request.InvoiceId.Value, ct);
            if (invoice == null)
                return ApiResponse<SuperQiPaymentResponse>.ErrorResponse("Invoice not found.");
            if (invoice.Status == InvoiceStatus.Paid)
                return ApiResponse<SuperQiPaymentResponse>.ErrorResponse("Invoice is already paid.");
        }

        var amount = invoice?.TotalAmount ?? request.Amount;
        var currency = "IQD";
        var orderDescription = request.OrderDescription ?? (invoice != null
            ? $"RentARide Invoice {invoice.Id}"
            : "Agreement payment");

        var paymentRequestId = request.InvoiceId.HasValue
            ? $"INV-{request.InvoiceId.Value}"
            : $"AGREEMENT-PAY-{Guid.NewGuid()}-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";

        try
        {
            var expiryTime = DateTime.UtcNow.AddMinutes(30).ToString("yyyy-MM-ddTHH:mm:sszzz").Replace("+00:00", "+00:00");
            var baseUrl = _configuration["APP_URL"] ?? "http://localhost:5022";

            var paymentRequest = new AlipayPaymentRequest
            {
                ProductCode = AGREEMENT_PAYMENT,
                PaymentRequestId = paymentRequestId,
                PaymentAuthCode = request.AccessToken,
                PaymentAmount = new AlipayAmount
                {
                    Currency = currency,
                    Value = amount.ToString("0")
                },
                Order = new AlipayOrder
                {
                    OrderDescription = orderDescription,
                    Buyer = new AlipayBuyer
                    {
                        ReferenceBuyerId = request.CustomerId
                    }
                },
                PaymentExpiryTime = expiryTime,
                PaymentNotifyUrl = baseUrl + SUPERQI_WEBHOOK_PATH
            };

            var paymentResponse = await _alipayService.PayAsync(paymentRequest, ct);

            var response = new SuperQiPaymentResponse
            {
                Amount = amount,
                PaymentId = paymentResponse.PaymentId
            };

            switch (paymentResponse.Result.ResultStatus)
            {
                case "S":
                    response.Success = true;
                    if (invoice != null)
                    {
                        invoice.SuperQiPaymentId = paymentResponse.PaymentId;
                        invoice.SuperQiPaymentRequestId = paymentRequestId;
                        await _dbContext.SaveChangesAsync(ct);
                    }
                    _logger.LogInformation("[SuperQiMiniApp] Agreement payment completed: {PaymentId}, InvoiceId={InvoiceId}", paymentResponse.PaymentId, request.InvoiceId);
                    break;
                case "U":
                    response.Success = false;
                    response.Error = "Payment status unknown. Backend should poll for status.";
                    _logger.LogWarning("[SuperQiMiniApp] Agreement payment status unknown");
                    break;
                default:
                    response.Success = false;
                    response.Error = paymentResponse.Result.ResultMessage ?? "Payment failed";
                    _logger.LogWarning("[SuperQiMiniApp] Agreement payment failed: {Message}", response.Error);
                    break;
            }

            return response.Success
                ? ApiResponse<SuperQiPaymentResponse>.SuccessResponse(response)
                : ApiResponse<SuperQiPaymentResponse>.ErrorResponse(response.Error ?? "Payment failed");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[SuperQiMiniApp] Agreement payment failed");
            return ApiResponse<SuperQiPaymentResponse>.ErrorResponse(ex.Message);
        }
    }

    public async Task<ApiResponse<SuperQiPaymentResponse>> CreatePaymentForInvoiceAsync(Guid invoiceId, string finishPaymentUrl, CancellationToken ct = default)
    {
        _logger.LogInformation("[SuperQiMiniApp] Creating payment for invoice: {InvoiceId}", invoiceId);

        var invoice = await _dbContext.Invoices
            .FirstOrDefaultAsync(i => i.Id == invoiceId, ct);
        if (invoice == null)
            return ApiResponse<SuperQiPaymentResponse>.ErrorResponse("Invoice not found.");
        if (invoice.Status == InvoiceStatus.Paid)
            return ApiResponse<SuperQiPaymentResponse>.ErrorResponse("Invoice is already paid.");

        var paymentRequestId = $"INV-{invoiceId}";
        var expiryTime = DateTime.UtcNow.AddMinutes(30).ToString("yyyy-MM-ddTHH:mm:sszzz").Replace("+00:00", "+00:00");
        var baseUrl = _configuration["APP_URL"] ?? "http://localhost:5022";

        try
        {
            var paymentRequest = new AlipayPaymentRequest
            {
                ProductCode = ONLINE_PURCHASE,
                PaymentRequestId = paymentRequestId,
                PaymentAmount = new AlipayAmount
                {
                    Currency = invoice.Currency,
                    Value = invoice.TotalAmount.ToString("0")
                },
                Order = new AlipayOrder
                {
                    OrderDescription = $"RentARide Invoice {invoiceId}",
                    Buyer = new AlipayBuyer
                    {
                        ReferenceBuyerId = invoice.UserId.ToString()
                    }
                },
                PaymentExpiryTime = expiryTime,
                PaymentRedirectUrl = finishPaymentUrl,
                PaymentNotifyUrl = baseUrl + SUPERQI_WEBHOOK_PATH
            };

            var paymentResponse = await _alipayService.PayAsync(paymentRequest, ct);

            var response = new SuperQiPaymentResponse
            {
                Amount = invoice.TotalAmount,
                PaymentId = paymentResponse.PaymentId
            };

            if (paymentResponse.RedirectActionForm?.RedirectUrl != null)
            {
                response.Success = true;
                response.PaymentUrl = paymentResponse.RedirectActionForm.RedirectUrl;
                invoice.SuperQiPaymentRequestId = paymentRequestId;
                await _dbContext.SaveChangesAsync(ct);
                _logger.LogInformation("[SuperQiMiniApp] Payment URL created for invoice {InvoiceId}: {PaymentId}", invoiceId, paymentResponse.PaymentId);
            }
            else if (paymentResponse.Result.ResultStatus == "S")
            {
                response.Success = true;
                invoice.SuperQiPaymentId = paymentResponse.PaymentId;
                invoice.SuperQiPaymentRequestId = paymentRequestId;
                await _dbContext.SaveChangesAsync(ct);
                _logger.LogInformation("[SuperQiMiniApp] Payment completed immediately for invoice {InvoiceId}: {PaymentId}", invoiceId, paymentResponse.PaymentId);
            }
            else
            {
                response.Success = false;
                response.Error = paymentResponse.Result.ResultMessage ?? "No redirect URL received from payment API";
                _logger.LogWarning("[SuperQiMiniApp] Payment failed for invoice {InvoiceId}: {Message}", invoiceId, response.Error);
            }

            return response.Success
                ? ApiResponse<SuperQiPaymentResponse>.SuccessResponse(response)
                : ApiResponse<SuperQiPaymentResponse>.ErrorResponse(response.Error ?? "Payment failed");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[SuperQiMiniApp] Create payment for invoice {InvoiceId} failed", invoiceId);
            return ApiResponse<SuperQiPaymentResponse>.ErrorResponse(ex.Message);
        }
    }

    // ========== Private Helpers ==========

    private async Task<AlipayRefundResponse?> PollRefundStatusAsync(string refundRequestId, CancellationToken ct)
    {
        const int maxAttempts = 12;
        const int intervalSeconds = 5;

        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            _logger.LogInformation("[SuperQiMiniApp] Polling refund status attempt {Attempt}/{Max}", attempt, maxAttempts);

            try
            {
                var inquiryResponse = await _alipayService.InquiryRefundAsync(refundRequestId, ct);

                if (inquiryResponse.Result.ResultStatus == "S")
                {
                    switch (inquiryResponse.RefundStatus)
                    {
                        case "SUCCESS":
                            return new AlipayRefundResponse
                            {
                                Result = new AlipayResult
                                {
                                    ResultCode = "SUCCESS",
                                    ResultStatus = "S",
                                    ResultMessage = "Success"
                                },
                                RefundId = inquiryResponse.RefundId,
                                RefundTime = inquiryResponse.RefundTime
                            };
                        case "FAIL":
                            return new AlipayRefundResponse
                            {
                                Result = new AlipayResult
                                {
                                    ResultCode = "REFUND_FAILED",
                                    ResultStatus = "F",
                                    ResultMessage = inquiryResponse.RefundFailReason
                                }
                            };
                        case "PROCESSING":
                            // Continue polling
                            break;
                    }
                }
                else if (inquiryResponse.Result.ResultStatus == "F" && inquiryResponse.Result.ResultCode == "REFUND_NOT_EXIST")
                {
                    return new AlipayRefundResponse
                    {
                        Result = new AlipayResult
                        {
                            ResultCode = "REFUND_NOT_EXIST",
                            ResultStatus = "F",
                            ResultMessage = "Refund not found in wallet system"
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "[SuperQiMiniApp] Refund inquiry attempt {Attempt} failed", attempt);
            }

            if (attempt < maxAttempts)
                await Task.Delay(intervalSeconds * 1000, ct);
        }

        _logger.LogWarning("[SuperQiMiniApp] Refund polling timeout - status still unknown");
        return null;
    }

    private static SuperQiRefundResponse BuildRefundResponse(AlipayRefundResponse refundResponse)
    {
        var response = new SuperQiRefundResponse
        {
            ResultStatus = refundResponse.Result.ResultStatus,
            ResultCode = refundResponse.Result.ResultCode,
            ResultMessage = refundResponse.Result.ResultMessage
        };

        switch (refundResponse.Result.ResultStatus)
        {
            case "S":
                response.Success = true;
                response.Status = "SUCCESS";
                response.RefundId = refundResponse.RefundId;
                response.RefundTime = refundResponse.RefundTime;
                break;
            case "U":
                response.Success = false;
                response.Status = "PENDING";
                response.Message = "Refund is being processed. Status is unknown.";
                break;
            case "F":
                response.Success = false;
                response.Status = "FAILED";
                response.Message = refundResponse.Result.ResultMessage;
                break;
        }

        return response;
    }

    private static SuperQiNotificationResponse BuildNotificationResponse(AlipayNotificationResponse notifResponse)
    {
        var response = new SuperQiNotificationResponse
        {
            ResultStatus = notifResponse.Result.ResultStatus,
            ResultCode = notifResponse.Result.ResultCode,
            ResultMessage = notifResponse.Result.ResultMessage
        };

        switch (notifResponse.Result.ResultStatus)
        {
            case "S":
            case "A":
                response.Success = true;
                response.Status = "SUCCESS";
                response.MessageId = notifResponse.MessageId;
                break;
            case "U":
                response.Success = false;
                response.Status = "UNKNOWN";
                response.Message = "Notification status is unknown. It may still be processed.";
                break;
            case "F":
                response.Success = false;
                response.Status = "FAILED";
                response.Message = notifResponse.Result.ResultMessage;
                break;
        }

        return response;
    }
}
