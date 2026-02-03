using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RentARide.Application.Interfaces.Auth;

namespace RentARide.Infrastructure.Services.Auth;

public class SuperQiUserResolver(
    IHttpClientFactory httpClientFactory,
    IConfiguration configuration,
    ILogger<SuperQiUserResolver> logger) : ISuperQiUserResolver
{
    public async Task<SuperQiUserInfo?> ResolveAsync(string authCode, CancellationToken cancellationToken = default)
    {
        var validationUrl = configuration["SuperQi:ValidationUrl"];

        if (string.IsNullOrWhiteSpace(validationUrl))
        {
            logger.LogWarning("SuperQi:ValidationUrl not configured");
            return null;
        }

        try
        {
            var client = httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync(validationUrl, new { token = authCode }, cancellationToken);
            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var userId = GetString(root, "customerId");

            if (string.IsNullOrEmpty(userId))
            {
                logger.LogWarning("SuperQi validation response did not contain userId");
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
            logger.LogError(ex, "SuperQi auth code validation failed");
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
