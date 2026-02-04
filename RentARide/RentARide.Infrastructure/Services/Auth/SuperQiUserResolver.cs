using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RentARide.Application.Interfaces.Auth;
using RentARide.Application.Interfaces.Services;

namespace RentARide.Infrastructure.Services.Auth;

/// <summary>
/// Resolves SuperQi auth code to user info.
/// Supports 3 modes:
/// 1. UseDemoUser - returns a demo user (for local development)
/// 2. UseAlipayService - uses ISuperQiAlipayService (Alipay+ applyToken + inquiryUserInfo)
/// 3. ValidationUrl - calls an external validation endpoint (legacy)
/// </summary>
public class SuperQiUserResolver : ISuperQiUserResolver
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<SuperQiUserResolver> _logger;
    private readonly ISuperQiAlipayService? _alipayService;

    public SuperQiUserResolver(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<SuperQiUserResolver> logger,
        ISuperQiAlipayService? alipayService = null)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
        _alipayService = alipayService;
    }

    public async Task<SuperQiUserInfo?> ResolveAsync(string authCode, CancellationToken cancellationToken = default)
    {
        var useDemoUser = _configuration.GetValue<bool>("SuperQi:UseDemoUser");
        var useAlipayService = _configuration.GetValue<bool>("SuperQi:UseAlipayService");
        var validationUrl = _configuration["SuperQi:ValidationUrl"];

        // Mode 1: Demo user (for local development)
        if (useDemoUser)
        {
            var suffix = authCode.Length >= 8 ? authCode[..8] : authCode;
            var userId = "demo-" + suffix;
            _logger.LogInformation("SuperQi:UseDemoUser - returning demo user {UserId}", userId);
            return new SuperQiUserInfo
            {
                UserId = userId,
                Email = $"{userId}@superqi.rentaride.local",
                FirstName = "SuperQi",
                LastName = "User"
            };
        }

        // Mode 2: Use Alipay+ service (recommended for production)
        if (useAlipayService && _alipayService != null)
        {
            return await ResolveViaAlipayServiceAsync(authCode, cancellationToken);
        }

        // Mode 3: Legacy validation URL
        if (!string.IsNullOrWhiteSpace(validationUrl))
        {
            return await ResolveViaValidationUrlAsync(authCode, validationUrl, cancellationToken);
        }

        _logger.LogWarning("SuperQi: No validation method configured (UseDemoUser, UseAlipayService, or ValidationUrl)");
        return null;
    }

    private async Task<SuperQiUserInfo?> ResolveViaAlipayServiceAsync(string authCode, CancellationToken ct)
    {
        try
        {
            _logger.LogInformation("SuperQi: Resolving via Alipay+ service");

            // Step 1: Exchange auth code for access token
            var tokenResponse = await _alipayService!.ApplyTokenAsync(authCode, ct);

            if (tokenResponse.Result.ResultCode != "SUCCESS")
            {
                _logger.LogWarning("SuperQi Alipay+ applyToken failed: {Code} - {Message}",
                    tokenResponse.Result.ResultCode, tokenResponse.Result.ResultMessage);
                return null;
            }

            if (string.IsNullOrEmpty(tokenResponse.AccessToken))
            {
                _logger.LogWarning("SuperQi Alipay+ applyToken returned no access token");
                return null;
            }

            // Step 2: Get user info using access token
            var userInfoResponse = await _alipayService.InquiryUserInfoAsync(tokenResponse.AccessToken, ct);

            var userId = userInfoResponse.UserInfo?.UserId ?? tokenResponse.CustomerId;

            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("SuperQi Alipay+ inquiryUserInfo returned no userId");
                return null;
            }

            _logger.LogInformation("SuperQi Alipay+ resolved user: {UserId}", userId);

            return new SuperQiUserInfo
            {
                UserId = userId,
                Email = $"{userId}@superqi.rentaride.local",
                FirstName = userInfoResponse.UserInfo?.UserName ?? "SuperQi",
                LastName = "User",
                AccessToken = tokenResponse.AccessToken,
                CustomerId = tokenResponse.CustomerId
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SuperQi Alipay+ auth code validation failed");
            return null;
        }
    }

    private async Task<SuperQiUserInfo?> ResolveViaValidationUrlAsync(string authCode, string validationUrl, CancellationToken ct)
    {
        try
        {
            _logger.LogInformation("SuperQi: Resolving via validation URL");

            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync(validationUrl, new { token = authCode }, ct);
            var json = await response.Content.ReadAsStringAsync(ct);
            var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var userId = GetString(root, "customerId");

            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("SuperQi validation response did not contain userId");
                return null;
            }

            return new SuperQiUserInfo
            {
                UserId = userId,
                Email = $"{userId}@superqi.rentaride.local",
                FirstName = "SuperQi",
                LastName = "User"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SuperQi auth code validation failed");
            return null;
        }
    }

    private static string? GetString(JsonElement el, params string[] path)
    {
        foreach (var p in path)
        {
            if (!el.TryGetProperty(p, out el))
                return null;
        }
        return el.GetString();
    }
}
