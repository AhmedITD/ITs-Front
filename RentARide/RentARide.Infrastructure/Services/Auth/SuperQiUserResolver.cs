using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RentARide.Application.Interfaces.Auth;

namespace RentARide.Infrastructure.Services.Auth;

/// <summary>
/// Resolves SuperQi auth code to user info by calling a configurable validation URL
/// or returning a demo user for development.
/// </summary>
public class SuperQiUserResolver(
    IHttpClientFactory httpClientFactory,
    IConfiguration configuration,
    ILogger<SuperQiUserResolver> logger) : ISuperQiUserResolver
{
    public async Task<SuperQiUserInfo?> ResolveAsync(string authCode, CancellationToken cancellationToken = default)
    {
        var validationUrl = configuration["SuperQi:ValidationUrl"];
        var useDemoUser = configuration.GetValue<bool>("SuperQi:UseDemoUser");

        if (useDemoUser)
        {
            logger.LogInformation("Using SuperQi demo user for auth code");
            return new SuperQiUserInfo
            {
                UserId = "superqi-demo",
                Email = "superqi-demo@rentaride.local",
                FirstName = "SuperQi",
                LastName = "Demo User"
            };
        }

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

            var userId = GetString(root, "userId") ?? GetString(root, "sub") ?? GetString(root, "id");
            var email = GetString(root, "email") ?? GetString(root, "data", "email");
            var firstName = GetString(root, "firstName") ?? GetString(root, "first_name") ?? GetString(root, "data", "firstName");
            var lastName = GetString(root, "lastName") ?? GetString(root, "last_name") ?? GetString(root, "data", "lastName");

            if (string.IsNullOrEmpty(userId))
            {
                var data = root.TryGetProperty("data", out var dataEl) ? dataEl : root;
                userId = GetString(data, "userId") ?? GetString(data, "sub") ?? GetString(data, "id");
                email ??= GetString(data, "email");
                firstName ??= GetString(data, "firstName");
                lastName ??= GetString(data, "lastName");
            }

            if (string.IsNullOrEmpty(userId))
            {
                logger.LogWarning("SuperQi validation response did not contain userId");
                return null;
            }

            return new SuperQiUserInfo
            {
                UserId = userId,
                Email = email ?? $"{userId}@superqi.rentaride.local",
                FirstName = firstName ?? "SuperQi",
                LastName = lastName ?? "User"
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
