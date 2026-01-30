namespace RentARide.Application.Common;

/// <summary>
/// Ensures a DateTime is suitable for PostgreSQL timestamp with time zone (UTC only).
/// JSON-bound dates from requests have Kind=Unspecified; Npgsql rejects those.
/// </summary>
public static class DateTimeExtensions
{
    public static DateTime ToUtc(this DateTime value)
    {
        if (value.Kind == DateTimeKind.Utc) return value;
        if (value.Kind == DateTimeKind.Local) return value.ToUniversalTime();
        return DateTime.SpecifyKind(value, DateTimeKind.Utc);
    }

    public static DateTime? ToUtc(this DateTime? value)
    {
        if (value == null) return null;
        return value.Value.ToUtc();
    }
}
