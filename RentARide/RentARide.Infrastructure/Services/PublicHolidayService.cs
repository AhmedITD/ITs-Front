using System.Text.Json;
using RentARide.Application.Interfaces.Services;

namespace RentARide.Infrastructure.Services;

public class PublicHolidayService(IHttpClientFactory httpClientFactory) : IPublicHolidayService
{
    private const string NagerDateBaseUrl = "https://date.nager.at/api/v3";
    private const string CountryCode = "DE";

    public async Task<bool> IsPublicHolidayAsync(DateTime date, CancellationToken cancellationToken = default)
    {
        var year = date.Year;
        var client = httpClientFactory.CreateClient();
        client.BaseAddress = new Uri(NagerDateBaseUrl);

        try
        {
            var response = await client.GetAsync($"/PublicHolidays/{year}/{CountryCode}", cancellationToken);
            if (!response.IsSuccessStatusCode)
                return false;

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var holidays = JsonSerializer.Deserialize<List<NagerHoliday>>(json);
            if (holidays == null)
                return false;

            var dateStr = date.ToString("yyyy-MM-dd");
            return holidays.Any(h => h.DateString == dateStr);
        }
        catch
        {
            return false;
        }
    }

    private sealed class NagerHoliday
    {
        [System.Text.Json.Serialization.JsonPropertyName("date")]
        public string DateString { get; set; } = string.Empty;
    }
}
