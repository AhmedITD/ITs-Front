using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RentARide.Application.DTOs.Requests.SuperQi;
using RentARide.Application.DTOs.Responses.SuperQi;
using RentARide.Application.Interfaces.Services;

namespace RentARide.Infrastructure.Services.SuperQi;

/// <summary>
/// Alipay+ (SuperQi) API client.
/// Port of backend-node/src/alipay/client.js and alipay.js.
/// </summary>
public class SuperQiAlipayService : ISuperQiAlipayService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<SuperQiAlipayService> _logger;
    
    private readonly string _gatewayUrl;
    private readonly string _clientId;
    private readonly RSA? _privateKey;

    public SuperQiAlipayService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<SuperQiAlipayService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
        
        _gatewayUrl = _configuration["SuperQiAlipay:GatewayUrl"] ?? string.Empty;
        _clientId = _configuration["SuperQiAlipay:ClientId"] ?? string.Empty;
        
        var privateKeyPath = _configuration["SuperQiAlipay:MerchantPrivateKeyPath"];
        if (!string.IsNullOrEmpty(privateKeyPath) && File.Exists(privateKeyPath))
        {
            _privateKey = LoadPrivateKey(privateKeyPath);
        }
        
        _httpClient.Timeout = TimeSpan.FromSeconds(25);
    }

    public async Task<AlipayTokenResponse> ApplyTokenAsync(string authCode, CancellationToken ct = default)
    {
        const string path = "/v1/authorizations/applyToken";
        var payload = new
        {
            grantType = "AUTHORIZATION_CODE",
            authCode
        };
        return await SendRequestAsync<AlipayTokenResponse>(path, payload, ct);
    }

    public async Task<AlipayUserInfoResponse> InquiryUserInfoAsync(string accessToken, CancellationToken ct = default)
    {
        const string path = "/v1/users/inquiryUserInfo";
        var payload = new { accessToken };
        
        return await SendRequestAsync<AlipayUserInfoResponse>(path, payload, ct);
    }

    public async Task<AlipayTokenResponse> InquiryUserCardListAsync(string accessToken, CancellationToken ct = default)
    {
        const string path = "/v1/users/inquiryUserCardList";
        var payload = new { accessToken };
        
        return await SendRequestAsync<AlipayTokenResponse>(path, payload, ct);
    }

    public async Task<AlipayPaymentResponse> PayAsync(AlipayPaymentRequest request, CancellationToken ct = default)
    {
        const string path = "/v1/payments/pay";
        
        _logger.LogInformation("[SuperQiAlipay] Creating payment: RequestId={RequestId}", request.PaymentRequestId);
        
        return await SendRequestAsync<AlipayPaymentResponse>(path, request, ct);
    }

    public async Task<AlipayRefundResponse> RefundAsync(AlipayRefundRequest request, CancellationToken ct = default)
    {
        const string path = "/v1/payments/refund";
        
        _logger.LogInformation("[SuperQiAlipay] Initiating refund: RequestId={RequestId}, PaymentId={PaymentId}", 
            request.RefundRequestId, request.PaymentId);
        
        return await SendRequestAsync<AlipayRefundResponse>(path, request, ct);
    }

    public async Task<AlipayRefundInquiryResponse> InquiryRefundAsync(string refundRequestId, CancellationToken ct = default)
    {
        const string path = "/v1/payments/inquiryRefund";
        var payload = new { refundRequestId };
        
        _logger.LogInformation("[SuperQiAlipay] Querying refund status: RefundRequestId={RefundRequestId}", refundRequestId);
        
        return await SendRequestAsync<AlipayRefundInquiryResponse>(path, payload, ct);
    }

    public async Task<AlipayNotificationResponse> SendInboxAsync(AlipayNotificationRequest request, CancellationToken ct = default)
    {
        const string path = "/v1/messages/sendInbox";
        
        _logger.LogInformation("[SuperQiAlipay] Sending inbox notification: RequestId={RequestId}", request.RequestId);
        
        return await SendRequestAsync<AlipayNotificationResponse>(path, request, ct);
    }

    public async Task<AlipayNotificationResponse> SendPushAsync(AlipayNotificationRequest request, CancellationToken ct = default)
    {
        const string path = "/v1/messages/sendPush";
        
        _logger.LogInformation("[SuperQiAlipay] Sending push notification: RequestId={RequestId}", request.RequestId);
        
        return await SendRequestAsync<AlipayNotificationResponse>(path, request, ct);
    }

    public async Task<AlipayPrepareAuthResponse> PrepareAuthorizationAsync(string contractDescription, CancellationToken ct = default)
    {
        const string path = "/v1/authorizations/prepare";
        
        var extendInfo = JsonSerializer.Serialize(new
        {
            language = "en-US",
            contractDesc = contractDescription
        });
        
        var payload = new
        {
            scopes = "AGREEMENT_PAY",
            extendInfo
        };
        
        _logger.LogInformation("[SuperQiAlipay] Preparing authorization for agreement");
        
        return await SendRequestAsync<AlipayPrepareAuthResponse>(path, payload, ct);
    }

    // ========== Private Helpers ==========

    private async Task<T> SendRequestAsync<T>(string path, object payload, CancellationToken ct) where T : new()
    {
        if (string.IsNullOrEmpty(_gatewayUrl) || string.IsNullOrEmpty(_clientId))
        {
            _logger.LogWarning("[SuperQiAlipay] Service not configured (missing GatewayUrl or ClientId)");
            return CreateErrorResponse<T>("SuperQiAlipay service not configured");
        }

        try
        {
            var jsonPayload = JsonSerializer.Serialize(payload, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });
            
            var requestTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz").Replace("+00:00", "+00:00");
            var signature = GenerateSignature("POST", path, requestTime, jsonPayload);
            
            var request = new HttpRequestMessage(HttpMethod.Post, _gatewayUrl + path)
            {
                Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
            };
            
            request.Headers.Add("Client-Id", _clientId);
            request.Headers.Add("Request-Time", requestTime);
            request.Headers.Add("Signature", $"algorithm=RSA256, keyVersion=1, signature={signature}");
            
            _logger.LogDebug("[SuperQiAlipay] Request: POST {Path}", path);
            
            var response = await _httpClient.SendAsync(request, ct);
            var responseBody = await response.Content.ReadAsStringAsync(ct);
            
            _logger.LogDebug("[SuperQiAlipay] Response: {StatusCode} - {Body}", response.StatusCode, responseBody);
            
            var result = JsonSerializer.Deserialize<T>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            
            return result ?? new T();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[SuperQiAlipay] Request failed: {Path}", path);
            return CreateErrorResponse<T>(ex.Message);
        }
    }

    private string GenerateSignature(string httpMethod, string path, string requestTime, string content)
    {
        if (_privateKey == null)
        {
            _logger.LogWarning("[SuperQiAlipay] Private key not loaded, returning empty signature");
            return string.Empty;
        }

        // Format: {HTTP_METHOD} {PATH}\n{CLIENT_ID}.{REQUEST_TIME}.{CONTENT}
        var signContent = $"{httpMethod} {path}\n{_clientId}.{requestTime}.{content}";
        var signBytes = Encoding.UTF8.GetBytes(signContent);
        
        var signatureBytes = _privateKey.SignData(signBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        return Convert.ToBase64String(signatureBytes);
    }

    private static RSA? LoadPrivateKey(string path)
    {
        try
        {
            var keyPem = File.ReadAllText(path);
            var rsa = RSA.Create();
            rsa.ImportFromPem(keyPem);
            return rsa;
        }
        catch
        {
            return null;
        }
    }

    private static T CreateErrorResponse<T>(string errorMessage) where T : new()
    {
        var response = new T();
        
        // Try to set Result property if it exists
        var resultProp = typeof(T).GetProperty("Result");
        if (resultProp != null)
        {
            var result = new AlipayResult
            {
                ResultCode = "SYSTEM_ERROR",
                ResultStatus = "F",
                ResultMessage = errorMessage
            };
            resultProp.SetValue(response, result);
        }
        
        return response;
    }
}
